;+
; :Author: Unknown
;-
; 转换影像文件为 ntf 格式
pro Tontf,infile,outfile
compile_opt IDL2

;初始化ENVI
envi,/Restore_Base_Save_files
ENVI_BATCH_INIT
e = ENVI(/HEADLESS)
raster = e.OpenRaster(infile)
;outfile=infile+"_bf.ntf"
print,outfile
e.ExportRaster, raster, outfile, 'NTF'
;print,raster
print,"finish"
end