; Author : Jacory Gao
; Description :
;   hyperspectral target detection methods.

pro Targetdetection
  input = 'E:\\Git\\test-data\\class-test-data\\can_tmr.img'
  output = 'E:\Git\test-data\res\detection_can_tmr.img'
  targetSpectral = [21.000000,28.000000,32.000000,35.000000,38.000000,30.000000]
  detectionMethod = 0
  
  Targetdetection_tt, input, output, detectionMethod, targetSpectral = targetSpectral
end

Pro Targetdetection_tt, input, output, detectionMethod, mode = mode, $
  targetSpectral = targetSpectral,    $ ; for CEM/ACE/MF/OSP
  kernelSize = kernelSize,            $ ; for RXD
  isLocal = isLocal                     ; for RXD

  Compile_opt idl2
  ENVI, /restore_base_save_files
  ENVI_BATCH_INIT

  If ~KEYWORD_SET(kernelSize) Then kernelSize = 15
  If ~KEYWORD_SET(isLocal) Then isLocal = 0
  if ~KEYWORD_SET(mode) Then mode = 0 ; default is single file mode
  
  filecount = 0
  filenames = ''
  outfilename = output
  If mode Eq 0 Then Begin ; single file mode
    filecount = 1
    filenames[0] = input
  Endif Else If mode Eq 1 Then Begin ; batch mode
    filenames = FILE_SEARCH(input, "*")
    filecount = N_ELEMENTS(filenames)
  Endif

  For fileIndex = 0L, filecount - 1 Do Begin
    ENVI_OPEN_FILE, filenames[fileIndex], r_fid = fid
    ENVI_FILE_QUERY, fid, dims = dims,  $
      fname = fname,                    $
      sname = sname,                    $
      data_type = data_type,            $
      interleave = interleave,          $
      nb = nb,                          $; num of bands
      nl = nl,                          $; num of lines
      ns = ns                            ; num of samples

    if filecount ne 1 then outfilename = output + sname + '_res'
  
    methodString = ''
    Switch (detectionMethod) Of
      0: Begin
        methodString = 'ENVI_CEM_DOIT'
        Break
      End
      1: Begin
        methodString = 'ENVI_ACE_DOIT'
        Break
      End
      2: Begin
        methodString = 'ENVI_RXD_DOIT'
        Break
      End
      3: Begin
        methodString = 'ENVI_OSP_DOIT'
        Break
      End
      else: begin
        message, 'error, function is not defined.'
      end
    Endswitch

    Switch (detectionMethod) Of
      0:
      1:
      2: Begin
        ENVI_DOIT, methodString,          $
          fid = fid,                      $
          pos = INDGEN(nb),               $
          dims = dims,                    $
          out_name = outfilename,         $
          out_bname = 'detection result', $
          r_fid = rfid,                   $
          target = targetSpectral
          
        break
      End

      3: Begin
        ENVI_DOIT, 'envi_rxd_doit',          $
          fid = fid,                      $
          dims = dims,                    $
          pos = lINDGEN(nb),               $
          out_name = outfilename,         $
          out_bname = 'detection result', $
          r_fid = rfid,                   $
          kernel_size = kernelSize,       $
          local = isLocal,                $
          method = 0
          
        break
      End
    Endswitch
    
    if rfid ne -1 then print, 'Done'
  Endfor


  ENVI_BATCH_EXIT
End