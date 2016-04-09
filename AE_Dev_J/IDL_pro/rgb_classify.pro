; Author : Jacory Gao
; extract some basic classes from RGB image file

FUNCTION Removefid, fid
  COMPILE_OPT idl2

  IF fid EQ -1 THEN Return, 0
  Envi_file_mng, id = fid, /remove
  Return, 1
END

PRO Constructclassfile, infid, className, $
  r_fid = r_fid

  COMPILE_OPT idl2
  Envi, /RESTORE_BASE_SAVE_FILES
  Envi_batch_init

  Envi_file_query, infid,     $
    nb = nb,                  $
    ns = ns,                  $
    nl = nl,                  $
    dims = dims,              $
    data_type = d_type

  mapinfo = Envi_get_map_info(fid = infid)

  bnames = ['Classification']
  file_type = ENVI_FILE_TYPE('ENVI Classification')
  class_names = ['Unclassified', className]
  lookup = [[0,0,0], [255,0,0]]

  tmpOut = Filepath('tmpOut_'+ className, /tmp)
  Envi_doit, 'cf_doit',       $
    fid = infid,              $
    dims = dims,              $
    out_bname = [className],    $
    pos = Lindgen(nb),        $
    out_name = tmpOut,        $
    r_fid = r_fid
  IF r_fid EQ -1 THEN BEGIN
    Print, 'construct class file failed in cf_doit'
    Return
  ENDIF

  Envi_setup_head, fname = tmpOut,      $
    nl = nl, ns = ns, nb = 1,           $
    interleave = 0, data_type = d_type, $
    map_info = mapinfo,                 $
    class_names = class_names,          $
    FILE_TYPE = file_type,              $
    lookup = lookup,                    $
    bnames = bnames,                    $
    num_classes = 2,                    $
    descrip = descrip,                  $
    /write

  t = Removefid(r_fid)
  Envi_open_file, tmpOut, r_fid = r_fid
END

PRO Postclassification, infid, className, r_fid = r_fid

  Envi_file_query, infid,     $
    nb = nb,                  $
    ns = ns,                  $
    nl = nl,                  $
    dims = dims

  tmpfinal_maj = Filepath('tmpfinalMaj_'+ className, /tmp)
  Envi_doit, 'Class_Majority_Doit',             $
    FID = infid,                       $
    DIMS = dims,                                $
    POS = [0],                                  $
    METHOD = 0,                                 $
    KERNEL_SIZE = [5,5],                        $
    CENTER_WEIGHT = 1,                          $
    CLASS_PTR = [1],                          $
    OUT_bNAME = 'Maj',                     $
    out_name = tmpfinal_maj,                $
    r_fid = r_m_fid
  IF r_m_fid EQ -1 THEN BEGIN
    Print, 'post classification failed in majority_doit'
    r_fid = -1
    Return
  ENDIF

  tmpSeive = Filepath('tmpSeive_'+ className, /tmp)
  Envi_doit, 'class_cs_doit',           $
    fid = r_m_fid,                      $
    dims = dims,                        $
    pos = [0],                          $
    /eight,                             $
    out_bname = className + '_seive band',           $
    method = 1,                         $
    out_name = tmpSeive,                $
    r_fid = seive_fid,                  $
    sieve_min = 10
  IF seive_fid EQ -1 THEN BEGIN
    Print, 'post classification failed in seive_doit'
    r_fid = -1
    Return
  ENDIF

  r_fid = seive_fid
END

PRO Rgb_classify, input, output, processMode
  COMPILE_OPT idl2
  Envi, /RESTORE_BASE_SAVE_FILES
  Envi_batch_init

  ;  fileDir = Dialog_pickfile(/directory, title = "Select folder")
  ;  IF (fileDir EQ '') THEN Return
  ;
  ;  outDir = Dialog_pickfile(/directory, title = "Select result folder");
  ;  IF (outDir EQ '') THEN Return
  ;

  ;fileDir = 'E:\Jacory\3_4航片处理\无人机高分影像30景样本\050'
  ;outDir = 'E:\Jacory\3_4航片处理\无人机高分影像30景样本\test\'

  filenames = input
  outfilename = output
  IF processMode EQ 'singleFileMode' THEN BEGIN
    filenames = input
    outfilename = output
  ENDIF ELSE BEGIN
    filenames = File_search(input, "*.tif");
    outfilename = output
  END
  FOR fileIndex = 0L, N_elements(filenames)-1 DO BEGIN
    filename = filenames[fileIndex];
    IF filename EQ '' THEN CONTINUE

    ;extract the base name of the image
    temp = Strsplit(filename,"\",/extract);
    temp2 = Strsplit(temp[N_elements(temp)-1],".",/extract);
    basename = Strjoin(temp2[0:N_elements(temp2)-2]);
    IF (temp2[N_elements(temp2)-1] EQ 'hdr' OR $
      temp2[N_elements(temp2)-1] EQ 'HDR') THEN CONTINUE

    if processMode eq 'multiFileMode' then outfilename = outfilename + basename

    Envi_open_file, filename, r_fid = img_fid, /NO_REALIZE
    Envi_file_query, img_fid, $
      dims = dims,            $
      ns = ns,                $
      nl = nl,                $
      nb = nb

    ; 注意： 这里没有分块
    R_data = Fix( Envi_get_data( fid = img_fid, dims = dims, pos = 0 ) )
    G_data = Fix( Envi_get_data( fid = img_fid, dims = dims, pos = 1 ) )
    B_data = Fix( Envi_get_data( fid = img_fid, dims = dims, pos = 2 ) )

    Veg_data = Intarr(ns, nl)
    build_data = Intarr(ns, nl)
    water_data = Intarr(ns, nl)

    ;==== 利用RGB的值判断植被 ====
    veg_index = Where(G_data GT R_data AND $
      G_data GT B_data AND $
      G_data LE 150 AND    $
      R_data LE 80 AND     $
      R_data LE 80,        $
      COUNT)
    IF COUNT GT 0 THEN BEGIN
      veg_data[veg_index] = 1
    ENDIF

    ;==== 裸土,建筑等, 两种规则方式，视情况选择
    build_index = Where($
      G_data GT 150 AND $
      G_data LT 230 AND $
      R_data LT 205 AND $
      R_data GT 80 AND  $
      R_data LT 215 AND $
      B_data LT 80,     $
      COUNT)
    build_index2 = Where($
      G_data GT 150 AND $
      (Abs(G_data-R_data) LE 50) AND  $
      (Abs(G_data-B_data) LE 50),     $
      COUNT)
    IF COUNT GT 0 THEN BEGIN
      build_data[build_index2] = 1
    ENDIF

    ; 利用RGB值判断水体, 排除 build 和  veg 部分
    water_index = Where(    $
      G_data GE R_data AND  $
      B_data GE R_data AND  $
      build_data NE 1 AND   $
      veg_data NE 1,        $
      COUNT)
    IF COUNT GT 0 THEN BEGIN
      water_data[water_index] = 1
    ENDIF

    Envi_enter_data, veg_data,  $
      bname = 'vegetation',     $
      r_fid = vegetation_fid

    Envi_enter_data, build_data,  $
      bname = 'building',         $
      r_fid = building_fid

    Envi_enter_data, water_data,  $
      bname = 'water',            $
      r_fid = water_fid

    ;===== 构造分类图 ====
    Constructclassfile, building_fid, 'build', r_fid = build_class_fid
    Constructclassfile, vegetation_fid, 'vegetation', r_fid = veg_class_fid
    Constructclassfile, water_fid, 'water', r_fid = water_class_fid

    t = Removefid(building_fid)
    t = Removefid(vegetation_fid)
    t = Removefid(water_fid)

    ; ==== 分类后处理 ====
    Postclassification, build_class_fid, 'build', r_fid = build_final_fid
    Postclassification, veg_class_fid, 'vegetation', r_fid = veg_final_fid
    Postclassification, water_class_fid, 'water', r_fid = water_final_fid

    t = Removefid(build_class_fid)
    t = Removefid(veg_class_fid)
    t = Removefid(water_class_fid)

    IF build_final_fid EQ -1 OR $
      veg_final_fid EQ -1 OR $
      water_final_fid EQ -1 THEN BEGIN
      Print, 'process failed'
      Return
    ENDIF

    ; ==== 构造结果文件 ====
    Envi_doit, 'cf_doit',       $
      fid = [build_final_fid, veg_final_fid, water_final_fid], $
      dims = dims,              $
      pos = [0,0,0],            $
      out_name = outfilename,       $
      out_bname = ["build","veg","water"], $
      r_fid = tmpOutfid

    t = Removefid(tmpOutfid)
  ENDFOR
  Print, 'Done'
END


