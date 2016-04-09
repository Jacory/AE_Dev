;+
; :Author: Jacory Gao
;  用于对分类文件进行统计
;-
pro class_statistics, infile, reportfile
  compile_opt idl2
  Envi, /RESTORE_BASE_SAVE_FILES
  Envi_batch_init
  
  Envi_open_file, infile, r_fid = infid
  Envi_file_query, infid,     $
    nb = nb,                  $
    ns = ns,                  $
    nl = nl,                  $
    dims = dims,              $
    file_type = f_type,       $
    num_classes = num_classes

  IF f_type NE 3 THEN Return ; not a valid ENVI Classification file
  if num_classes eq 0 then return   ;no class in this file
  
  envi_doit, 'Class_Stats_Doit',        $
    fid = infid,                        $
    class_fid = infid,                  $
    class_dims = dims,                  $
    class_ptr = lindgen(num_classes),   $
    comp_flag = 1,                      $
    report_flag = 1,                    $
    rep_name = reportfile,              $
    pos = lindgen(nb)
    
  Envi_batch_exit
end