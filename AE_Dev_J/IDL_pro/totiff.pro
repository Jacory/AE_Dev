;+
; :Author: Unknown
;-
; 转换影像文件为 .tif 格式
pro Totiff,infile,outfile
compile_opt IDL2
e = ENVI(/HEADLESS)
raster = e.OpenRaster(infile)
e.ExportRaster, raster, outfile, 'TIFF'
end