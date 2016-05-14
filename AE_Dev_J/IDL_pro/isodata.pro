; Author : Jacory Gao
; Description : ISODATA classification process
;
PRO ISODATA, input, output,   $
  iteration,                  $ ; 迭代次数
  change_thresh,              $ ; 变化阀值   0到1
  iso_merge_dist,             $ ; 类均值之间的最小距离
  iso_merge_pairs,            $ ; 合并成对的最大数
  iso_min_pixels,             $ ; 类最少象元数
  iso_split_std,              $ ; 最大分类标准差
  min_classes,                 $ ; 最少类别数
  mode = mode                  ; single file process or batch process

  COMPILE_OPT idl2
  ENVI, /restore_base_save_files
  ENVI_BATCH_INIT, NO_STATUS_WINDOW = 1- keyWord_set(showProcess)

  ; set default parameters
  IF ~KEYWORD_SET(change_thresh) THEN change_thresh = .05
  IF ~KEYWORD_SET(num_classes) THEN num_classes = 10
  IF ~KEYWORD_SET(iterations) THEN iterations = 1
  IF ~KEYWORD_SET(iso_merge_dist) THEN iso_merge_dist = 1
  IF ~KEYWORD_SET(iso_merge_pairs) THEN iso_merge_pairs = 2
  IF ~KEYWORD_SET(iso_min_pixels) THEN iso_min_pixels = 1
  IF ~KEYWORD_SET(iso_split_std) THEN iso_split_std = 1
  IF ~KEYWORD_SET(min_classes) THEN min_classes = 5
  if ~keyword_set(mode) then mode = 0 

  If mode Eq 0 Then Begin ; single file mode
    filecount = 1
    filenames[0] = input
  Endif Else If mode Eq 1 Then Begin ; batch mode
    filenames = FILE_SEARCH(input, "*")
    filecount = N_ELEMENTS(filenames)
  Endif
  
  for fileIndex = 0L, filecount - 1 Do Begin
    
    ;open process file
    ENVI_OPEN_FILE, filenames[fileIndex], r_fid = fid
    IF (fid EQ -1) THEN BEGIN
      RETURN
    ENDIF

    ;get file information
    ENVI_FILE_QUERY, fid, dims = dims,  $
      fname = fname,                    $
      sname = sname,                    $
      data_type = data_type,            $
      interleave = interleave,          $
      nb = nb,                          $; num of bands
      nl = nl,                          $; num of lines
      ns = ns                            ; num of samples
    
    if filecount ne 1 then outfilename = output + sname + '_res'

    ENVI_DOIT, 'class_doit',                $
      fid = fid,                            $
      pos = lindgen(nb),                    $
      dims = dims,                          $
      out_bname = 'isodata',                $
      out_name = outfilename,               $
      method = 4,                           $
      r_fid = r_fid,                        $
      num_classes = num_classes,            $
      iterations = iterations,              $
      change_thresh = change_thresh,        $
      iso_merge_dist = iso_merge_dist,      $
      iso_merge_pairs = iso_merge_pairs,    $
      iso_min_pixels = iso_min_pixels,      $
      iso_split_smult = 1,                  $
      iso_split_std = iso_split_std,        $
      min_classes = min_classes
  endfor
END