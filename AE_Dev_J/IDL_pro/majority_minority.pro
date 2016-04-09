pro majority_minority, input, output, methodType, kSizeX, KSizeY 
  COMPILE_OPT idl2
  Envi, /RESTORE_BASE_SAVE_FILES
  Envi_batch_init
  
  Envi_open_file, input, r_fid = infid
  
  Envi_file_query, infid,     $
    nb = nb,                  $
    ns = ns,                  $
    nl = nl,                  $
    dims = dims,              $
    file_type = f_type     
  
  if f_type ne 3 then return ; not a valid ENVI Classification file

  method = 0
  if methodType eq 'Minority' then method = 1
  

  Envi_doit, 'Class_Majority_Doit',             $
    FID = infid,                                $
    DIMS = dims,                                $
    POS = [0],                                  $
    METHOD = method,                            $
    KERNEL_SIZE = [kSizeX, KSizeY],             $
    CENTER_WEIGHT = 1,                          $
    CLASS_PTR = [1],                            $
    OUT_bNAME = 'Maj',                          $
    out_name = output,                          $
    r_fid = r_m_fid
  IF r_m_fid EQ -1 THEN BEGIN
    Print, 'post classification failed in majority_doit'
    r_fid = -1
    Return
  ENDIF
end