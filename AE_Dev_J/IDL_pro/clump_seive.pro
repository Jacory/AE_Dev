pro clump_seive, input, output, methodType, kSizeX, KSizeY
  COMPILE_OPT idl2
  Envi, /RESTORE_BASE_SAVE_FILES
  Envi_batch_init
  
  envi_open_file, input, r_fid = infid
  Envi_file_query, infid,     $
    nb = nb,                  $
    ns = ns,                  $
    nl = nl,                  $
    dims = dims,              $
    file_type = f_type
 
 if f_type ne 3 then return ; not a valid ENVI Classification file
 
  
  IF methodType EQ 'Seive' THEN begin
    Envi_doit, 'class_cs_doit',           $
      fid = infid,                        $
      dims = dims,                        $
      pos = [0],                          $
      /eight,                             $
      out_bname = 'seive band',           $
      METHOD = 1,                         $
      out_name = output,                  $
      r_fid = seive_fid,                  $
      sieve_min = kSizeX * kSizeY

    IF seive_fid EQ -1 THEN BEGIN
      Print, 'post classification failed in seive_doit'
      r_fid = -1
      Return
  ENDIF
  endif else if methodType eq 'Clump' then begin
    Envi_doit, 'Class_CS_Doit',             $
      FID = infid,                          $
      DIMS = dims,                          $
      POS = [0],                            $
      DKERN = Bytarr(kSizeX, kSizeY) + 1b,  $
      EKERN = Bytarr(kSizeX, kSizeY) + 1b,  $
      METHOD = 0,                           $
      OUT_NAME = output,                    $
      R_FID = clump_fid

    IF clump_fid EQ -1 THEN BEGIN
      Print, 'post classification failed in clump_doit'
      r_fid = -1
      Return
    ENDIF
  endif
end