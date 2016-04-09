PRO ISODATA,inputfile,outputfile, method,$
    ;ISO算法参数
    CHANGE_THRESH, $;变化阀值   0到1
    ISO_MERGE_DIST, $；类均值之间的最小距离
    ISO_MERGE_PAIRS, $;合并成对的最大数
    ISO_MIN_PIXELS, $；类最少象元数
    ISO_SPLIT_SMULT, $；设置类均值的标准差
    ISO_SPLIT_STD, $；最大分类标准差
    MIN_CLASSES;最少类别数
    
  COMPILE_OPT idl2
  CATCH, Error_status
  ;errorshow = 'Sorry to see the error,'+ $
   ; ' please send the error Information to "dongyq@esrichina-bj.cn"'
  IF Error_status NE 0 THEN BEGIN
   ; tmp = DIALOG_MESSAGE(errorshow+STRING(13b)+$
    ;  !ERROR_STATE.MSG,/error,title = '错误提示!')
    return
  ENDIF
  
  ENVI, /restore_base_save_files
  ENVI_BATCH_INIT, NO_STATUS_WINDOW = 1- keyWord_set(showProcess)
  ;输入数据预处理
  ENVI_OPEN_FILE, inputfile, r_fid=fid
  IF (fid EQ -1) THEN BEGIN
    RETURN
  ENDIF
  ;获取文件信息
  ENVI_FILE_QUERY, fid, dims=dims, nb=nb
  pos  = LINDGEN(nb)
  out_name = outputfile + ".dat"
      IF ~KEYWORD_SET(CHANGE_THRESH) THEN CHANGE_THRESH = .05
      IF ~KEYWORD_SET(NUM_CLASSES) THEN NUM_CLASSES = 10
      IF ~KEYWORD_SET(ITERATIONS) THEN ITERATIONS = 1
      IF ~KEYWORD_SET(ISO_MERGE_DIST) THEN ISO_MERGE_DIST = 1
      IF ~KEYWORD_SET(ISO_MERGE_PAIRS) THEN ISO_MERGE_PAIRS = 2
      IF ~KEYWORD_SET(ISO_MIN_PIXELS) THEN ISO_MIN_PIXELS = 1
      IF ~KEYWORD_SET(ISO_SPLIT_SMULT) THEN ISO_SPLIT_SMULT = 1
      IF ~KEYWORD_SET(ISO_SPLIT_STD) THEN ISO_SPLIT_STD = 1
      IF ~KEYWORD_SET(MIN_CLASSES) THEN MIN_CLASSES = 5
      out_bname = 'IsoData' 
      ENVI_DOIT, 'class_doit', fid=fid, pos=pos, dims=dims, $
        out_bname=out_bname, out_name=out_name, method=method, $
        r_fid=r_fid, $
        NUM_CLASSES = NUM_CLASSES, $
        ITERATIONS = ITERATIONS, $
        CHANGE_THRESH = CHANGE_THRESH, $
        ISO_MERGE_DIST = ISO_MERGE_DIST, $
        ISO_MERGE_PAIRS = ISO_MERGE_PAIRS, $
        ISO_MIN_PIXELS = ISO_MIN_PIXELS, $
        ISO_SPLIT_SMULT = ISO_SPLIT_SMULT, $
        ISO_SPLIT_STD = ISO_SPLIT_STD, $
        MIN_CLASSES = MIN_CLASSES 
END