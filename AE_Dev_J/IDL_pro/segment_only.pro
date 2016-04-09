;+
; :Author: Unknown
; object oriented segmentation
;-
PRO SEGMENT_ONLY ,infile,outfile,segment,merge
compile_opt IDL2
;
; Initialize ENVI and send all errors 
; and warnings to the file batch.txt 
; 
e = ENVI(/HEADLESS)
e.LOG_FILE = 'batch.txt' 
; 
; Open the input file 
; 
raster = e.OpenRaster(infile)
fid = ENVIRasterToFID(raster)
;
; Verify FID is valid
;
IF (FID eq -1) THEN BEGIN
  e.Close
  RETURN
ENDIF
;
; Set output filenames
;
SegImage = FILE_DIRNAME(outfile) + PATH_SEP() + FILE_BASENAME(outfile + ".dat")
;SegImage = 'segmentation_image.dat'
;Report = SegImage + 'report.txt'
VectorImage = SegImage + 'regions.shp'
;AttImage = SegImage + 'attributes.dat'
; 
; Set the keywords.
dims = [-1L, 0, raster.ncolumns-1, 0, raster.nrows-1]
pos = lindgen(raster.nbands); process all bands
; 
; Perform the segmentation 
; 
envi_doit, 'envi_fx_segmentonly_doit', $ 
   fid=fid, pos=pos, dims=dims, $ 
   r_fid=r_fid, KERNEL_SIZE=3,$
   attribute_raster_filename=AttImage, $
   merge_level=merge, $
   report_filename=Report, scale_level=segment, $
   segmentation_raster_filename=SegImage, $
   vector_filename=VectorImage
; Exit ENVI 
e.Close
END
