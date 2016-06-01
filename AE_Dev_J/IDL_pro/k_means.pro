PRO K_means, input, output,     $
  num_classes,                  $ 
  iterations,                   $
  change_thresh,                $
  mode

  COMPILE_OPT IDL2
  
  Catch, Error_status
  IF Error_status NE 0 THEN BEGIN
    Void = Dialog_message(!ERROR_STATE.Msg, /error)
    Catch, /CANCEL
    Return
  ENDIF

  Envi, /restore_base_save_files
  Envi_batch_init, NO_STATUS_WINDOW = 1- Keyword_set(showProcess)

  ; set default parameters
  IF ~KEYWORD_SET(change_thresh) THEN change_thresh = .05
  IF ~KEYWORD_SET(num_classes) THEN num_classes = 10
  IF ~KEYWORD_SET(iterations) THEN iterations = 1
  if ~keyword_set(mode) then mode = 0

  filenames = ''
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
    if filecount eq 1 then outfilename = output

    ENVI_DOIT, 'class_doit',                $
      fid = fid,                            $
      pos = lindgen(nb),                    $
      dims = dims,                          $
      out_bname = 'k-means',                $
      out_name = outfilename,               $
      method = 7,                           $
      r_fid = r_fid,                        $
      num_classes = num_classes,            $
      iterations = iterations,              $
      change_thresh = change_thresh,        $
      min_classes = min_classes
  endfor
END