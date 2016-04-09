;+
; :Author: Jacory Gao
; for testing other programs
;-
pro test_postClassify 
  input = 'C:\Users\Administrator\Desktop\test_postClassify.tif';
  output = 'C:\Users\Administrator\Desktop\resut_test.img';
  
  ;Majority_minority, input, output, 'Majority', 5, 5
 Clump_seive, input, output, 'Seive', 5, 5
  
end