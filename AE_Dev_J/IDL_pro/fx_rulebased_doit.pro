PRO FX_RULEBASED_DOIT, infile, rulefile, outfile, outshpefile, scale_level, merge_level

  COMPILE_OPT IDL2
  
  CATCH, Error_status
  IF Error_status NE 0 THEN BEGIN
    Void = DIALOG_MESSAGE(!ERROR_STATE.MSG, /error)
    CATCH, /CANCEL
    RETURN
  ENDIF
  
;  infile = 'D:\ENVIDemo\13. WorldView2信息提取\qb_colorado.dat'
;  rulefile = 'D:\ENVIDemo\13. WorldView2信息提取\建筑物\住宅 - 规则\屋顶-rule.rul'
;  outfile = 'D:\temp\try.dat'
  
  ;Input file and Output file
  ;file = filepath('bhtmref.img', root_dir = envi_get_path(), sub = ['data'])
  ;SegImage = envi_get_tmp()
  
  ;scale_level = 40.0
  ;merge_level = 90.0
  
  ENVI, /restore_base_save_files
  ENVI_BATCH_INIT

  ENVI_OPEN_FILE, infile, r_fid = fid
  ENVI_FILE_QUERY, fid, dims = dims, nb = nb
  pos = LINDGEN(nb)
  
  IF (FID EQ -1) THEN BEGIN
    ;    ENVI_BATCH_EXIT
    RETURN
  ENDIF
  
  tmpath = FILE_DIRNAME(outfile) + PATH_SEP() + FILE_BASENAME(outfile, '.dat')

  classification_raster_filename = outfile+".dat" 
  segmentation_raster_filename = outfile + 'segmentation.dat'
  vector_filename = outshpefile + '.shp'
  
  ENVI_DOIT, 'envi_fx_rulebased_doit',    $
    fid = fid,                            $
    pos = pos,                            $
    dims = dims,                          $
    r_fid = r_fid,                        $
    merge_level = merge_level,            $
    scale_level = scale_level,            $
    rule_filename = rulefile,             $
;    br_bands = [2,3],         $   ;计算NDVI
    segmentation_raster_filename = segmentation_raster_filename,      $
    classification_raster_filename = classification_raster_filename,  $
    vector_filename = vector_filename,    $
    /EXPORT_VECTOR_ATTRIBUTES

END