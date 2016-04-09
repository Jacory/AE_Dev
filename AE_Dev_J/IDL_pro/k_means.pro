PRO K_means, method, inputfile, outputfile, numclass, iteration, changeThresh

  COMPILE_OPT IDL2
  
  Catch, Error_status
  IF Error_status NE 0 THEN BEGIN
    Void = Dialog_message(!ERROR_STATE.Msg, /error)
    Catch, /CANCEL
    Return
  ENDIF

  Envi, /restore_base_save_files
  Envi_batch_init, NO_STATUS_WINDOW = 1- Keyword_set(showProcess)

  ;inputfile = "D:\Trainning\001.tif"
  ;outputfile = "D:\Trainning\005"
  ;ITERATIONS = 5
  ; NUM_CLASSES = 5

  Envi_open_file, inputfile, r_fid = fid
  Envi_file_query, fid, dims = dims, nb = nb

  pos = Lindgen(nb)
  out_name = outputfile + ".dat"
  
  IF (FID EQ -1) THEN return

  out_bname = 'K-Means'
  thresh = Replicate(0.05, numclass)

  Envi_doit, 'class_doit',          $
    fid = fid,                      $
    pos = pos,                      $
    dims = dims,                    $
    out_bname = out_bname,          $ 
    out_name = out_name,            $
    r_fid = r_fid,                  $
    METHOD = method,                $
    NUM_CLASSES = numclass,         $
    CHANGE_THRESH = changeThresh,   $
    ITERATIONS = iteration
END