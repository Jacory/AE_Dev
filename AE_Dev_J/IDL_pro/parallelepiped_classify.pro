PRO Parallelepiped_classify, method, infile, shapefile, outfile, singleValue
  COMPILE_OPT idl2
  Catch, Error_status
  IF Error_status NE 0 THEN BEGIN
    tmp = Dialog_message(errorshow+String(13b)+$
      !ERROR_STATE.Msg,/error,title = '错误提示!')
    Return
  ENDIF
  Envi, /restore_base_save_files
  
  ;ENVI_BATCH_INIT
  Envi_batch_init, NO_STATUS_WINDOW = 1- Keyword_set(showProcess)
  
  ;输入数据预处理
  Envi_open_file, infile, r_fid=fid
  IF (fid EQ -1) THEN BEGIN
    e.Close
    Return
  ENDIF
  
  ;获取文件信息
  Envi_file_query, fid, dims=dims, nb=nb,ns=ns,nl=nl
  pos  = Lindgen(nb)
  
  ;获得影像的坐标系统
  proj = Envi_get_projection(fid = fid)
  
  ;读取投影信息，并由此创建投影文件
  n = Strlen(shapefile)-3
  Strput, shapefile, "prj", n
  Openr, lun, shapefile, /get_lun
  shpPrjString = ''
  Readf, lun, shpPrjString
  Free_lun, lun
  shapfeProjection = Envi_proj_create(pe_coord_sys_str=shpPrjString)

  ;打开shp文件
  ;shapefile='D:\Trainning\ROI.shp'
  ;读取shp文件的信息
  Strput, shapefile, "shp", n
  oshp = Obj_new('IDLffShape', shapefile)
  oshp->Getproperty,              $
    n_entities = n_ent,           $
    Attribute_info = attr_info,   $
    n_attributes = n_attr,        $
    Entity_type = ent_type
  
  ;读实体
  FOR i = 0, n_ent - 1 DO BEGIN
    ent = oshp->Getentity(i) ;读取第i个实体
    ;获得实体的点坐标
    vert = *(ent.Vertices) ;实体的顶点
    ;读属性记录
    attr = oshp->Getattributes(i) ;读取第k个记录
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
      ELSE:color = -1

    ENDCASE
    IF color EQ -1 THEN CONTINUE
    roi_id = Envi_create_roi(ns=ns, nl=nl,color=color, name=attrName)
    Envi_convert_projection_coordinates, Reform(vert[0,*]), $
      Reform(vert[1,*]),  $
      shapfeProjection,   $
      oxmap, oymap,       $
      proj
    Envi_convert_file_coordinates, fid,   $
      x_file_coords,                      $
      y_file_coords,                      $
      oxmap, oymap    ;这里是转换成文件投影
    Envi_define_roi, roi_id, /POLYGON,xpts=x_file_coords,ypts=y_file_coords
    Envi_save_rois,attrName,roi_id
  ENDFOR
  Obj_destroy,oshp

  roi_ids = Envi_get_roi_ids(fid=fid,roi_colors=roi_colors, roi_names=class_names,nl=nl,ns=ns)
  class_names = ['Unclassified', class_names]

  num_classes = N_elements(roi_ids)
  
  ; Set the unclassified class to black and use roi colors
  lookup = Bytarr(3,num_classes+1)
  lookup[0,1] = roi_colors

  ; 计算类ROI的基本统计信息
  mean = Fltarr(N_elements(pos), num_classes)
  stdv = Fltarr(N_elements(pos), num_classes) + singleValue
  cov = Fltarr(N_elements(pos),N_elements(pos),num_classes)
  FOR j=0, num_classes-1 DO BEGIN
    roi_dims=[Envi_get_roi_dims_ptr(roi_ids[j]),0,0,0,0]
    Envi_doit, 'envi_stats_doit', fid=fid, pos=pos, $
      dims=roi_dims, comp_flag=4, mean=c_mean, $
      stdv=c_stdv, cov=c_cov
    Mean[0,j] = c_mean
    stdv[0,j] = c_stdv
    cov[0,0,j] = c_cov
  ENDFOR
  thresh = Replicate(0.05, num_classes)
  out_name = outfile + ".dat"
  out_bname = 'parallelepiped'
  Envi_doit, 'class_doit', fid=fid, pos=pos, dims=dims, $
    out_bname=out_bname, out_name=out_name, method=method, $
    mean=mean, stdv=stdv, std_mult=st_mult, $
    lookup=lookup, class_names=class_names, $
    in_memory=0, thresh=thresh
END