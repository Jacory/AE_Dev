pro Mahalanobis_classify,method,infile,shapefile,outfile,singleValue
compile_opt idl2
  CATCH, Error_status
  IF Error_status NE 0 THEN BEGIN
    tmp = DIALOG_MESSAGE(errorshow+STRING(13b)+$
    !ERROR_STATE.MSG,/error,title = '错误提示!')
    return
  ENDIF
  ENVI, /restore_base_save_files
  ;ENVI_BATCH_INIT
  ENVI_BATCH_INIT, NO_STATUS_WINDOW = 1- keyWord_set(showProcess)
  ;inputfile = "D:\Trainning\002.tif"
  ;输入数据预处理
  ENVI_OPEN_FILE, infile, r_fid=fid
  IF (fid EQ -1) THEN BEGIN
    RETURN
  ENDIF
  ;获取文件信息
  ENVI_FILE_QUERY, fid, dims=dims, nb=nb,ns=ns,nl=nl
  pos  = LINDGEN(nb)
  ;获得影像的坐标系统
  proj = ENVI_GET_PROJECTION(FID = fid)
  ;打开shp文件
  ;shapefile='D:\Trainning\ROI.shp'
  ;读取投影信息，并由此创建投影文件
  ;projstr='D:\Trainning\ROI.prj'
  n = strlen(shapefile)-3
  strput,shapefile,"prj",n
  openr,lun,shapefile,/get_lun
  shpPrjString=''
  readf,lun,shpPrjString
  free_lun,lun
  shapfeProjection=envi_proj_create(pe_coord_sys_str=shpPrjString)
  
  ;读取shp文件的信息
  strput,shapefile,"shp",n
  oshp=OBJ_NEW('IDLffShape',shapefile)
  oshp->getproperty,n_entities=n_ent,Attribute_info=attr_info,n_attributes=n_attr,Entity_type=ent_type
  ;读实体
  FOR i=0,n_ent-1 do begin ;循环
  ent=oshp->getentity(i) ;读取第i个实体
  ;获得实体的点坐标
  vert=*(ent.vertices) ;实体的顶点
  ;读属性记录
  attr=oshp->GetAttributes(i) ;读取第k个记录
  ;获得类别字段的记录
  attrName = attr.(0)
  attColor = attr.(1)
 CASE attColor OF
 '255,0,0': color = 2
'0,255,0': color = 3
'0,0,255': color = 4
'255,255,0':color = 5
 '0,255,255': color = 6
 '255,0,255': color = 7
 '176,48,96': color = 8
 '46,139,87':color = 9
'160,32,240':color = 10
 '255,127,80': color = 11
'127,255,212': color = 12
'218,112,214':color = 13
'160,82,45': color = 14
'127,255,0': color = 15
'216,191,216': color = 16
else:color=-1

endcase
    IF color EQ -1 THEN continue
  roi_id=envi_create_roi(ns=ns, nl=nl,color=color, name=attrName)
  ENVI_CONVERT_PROJECTION_COORDINATES, reform(vert[0,*]), reform(vert[1,*]),shapfeProjection,oxmap, oymap, proj
  ENVI_CONVERT_FILE_COORDINATES, fid, x_file_coords,y_file_coords, oxmap, oymap ;这里是转换成文件投影
  envi_define_roi, roi_id, /POLYGON,xpts=x_file_coords,ypts=y_file_coords
  envi_save_rois,attrName,roi_id
  print,i
  endfor
  Obj_destroy,oshp

  roi_ids = ENVI_GET_ROI_IDS(fid=fid,roi_colors=roi_colors, roi_names=class_names,nl=nl,ns=ns)
  class_names = ['Unclassified', class_names]

  num_classes = N_ELEMENTS(roi_ids)
 ; Set the unclassified class to black and use roi colors
  lookup = BYTARR(3,num_classes+1)
  lookup[0,1] = roi_colors
  
  ; 计算类ROI的基本统计信息
  mean = FLTARR(N_ELEMENTS(pos), num_classes)
  stdv = FLTARR(N_ELEMENTS(pos), num_classes) + singleValue
  cov = FLTARR(N_ELEMENTS(pos),N_ELEMENTS(pos),num_classes)
  FOR j=0, num_classes-1 DO BEGIN
     roi_dims=[ENVI_GET_ROI_DIMS_PTR(roi_ids[j]),0,0,0,0]
     ENVI_DOIT, 'envi_stats_doit', fid=fid, pos=pos, $
       dims=roi_dims, comp_flag=4, mean=c_mean, $
       stdv=c_stdv, cov=c_cov
  MEAN[0,j] = c_mean
  stdv[0,j] = c_stdv
  cov[0,0,j] = c_cov
  ENDFOR
  
  thresh=REPLICATE(0.05,num_classes)
  out_name = outfile + ".dat"
  out_bname = 'Mahalanobis'
  print,"last"
  ENVI_DOIT, 'class_doit', fid=fid, pos=pos, dims=dims, $
        out_bname=out_bname, out_name=out_name, method=2, $
        mean=mean, stdv=stdv, std_mult=st_mult, $
        lookup=lookup, class_names=class_names, $
        cov = cov,in_memory=0
 print,"fin"
end