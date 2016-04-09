PRO ENVI_FX_SEGMENTONLY_DOIT_RECORD, _extra = extra

END
PRO EXAMPLEBASED_DOIT,segType,megType,method,infile, shapefile, $
outRasterfile,outShapefile,segment,merge,knn_neighbors,threshold,$
;SVM分类方法
kernelType,kernelBias,kernerPanalty
compile_opt IDL2
; Initialize ENVI and send all errors 
; and warnings to the file batch.txt 
e = ENVI(/HEADLESS)
temp_dir= FILE_DIRNAME(outRasterfile) + PATH_SEP() + FILE_BASENAME(outRasterfile)
shp_dir = FILE_DIRNAME(outShapefile) + PATH_SEP() + FILE_BASENAME(outShapefile)
; Open the input file 
raster = e.OpenRaster(infile)
fid = ENVIRasterToFID(raster)
; Verify FID is valid
IF (FID eq -1) THEN BEGIN
  e.Close
  RETURN
ENDIF
; Set output filenames
report_filename = temp_dir+'report.txt'
confidence_raster_filename = temp_dir+'confidence.dat'
classification_raster_filename = temp_dir+'.dat'
segmentation_raster_filename = temp_dir+'segmentation.dat'
vector_filename = shp_dir+'.shp'
; 
; Set the keywords.
dims = [-1L, 0, raster.ncolumns, 0, raster.nrows]
pos = lindgen(raster.nbands); process all bands 
; Perform example-based classification 
CASE method OF
  'KNN': BEGIN
   envi_doit, 'envi_fx_examplebased_doit', fid=fid, pos=pos, dims=dims, $ 
   r_fid=r_fid, merge_level=merge,scale_level=segment, $
   classiication_algorithm="KNN",knn_neighbors = knn_neighbors, $
   CLASSIFICATION_THRESHOLD=threshold,$
   example_vector_filename=shapefile,$
   segment_bands=pos,segmentation_algorithm = segType,$
   ;MERGE_BANDS=pos,MERGE_ALGORITHM = megType,$
   segmentation_raster_filename=segmentation_raster_filename, $
   report_filename=report_filename, $
   confidence_raster_image=confidence_raster_filename, $
   classification_raster_filename=classification_raster_filename, $
   vector_filename=vector_filename  
  END
  'SVM':BEGIN
  case kernelType of
  'Radial Basis':begin
   envi_doit, 'envi_fx_examplebased_doit', fid=fid, pos=pos, dims=dims, $ 
   r_fid=r_fid, merge_level=merge,scale_level=segment, $
   classiication_algorithm=method,kernel_type = kernelType,$
   CLASSIFICATION_THRESHOLD=threshold,penalty = kernerPanalty,$
   example_vector_filename=shapefile, $
   segment_bands=pos,segmentation_algorithm = segType,$
   ;MERGE_BANDS=pos,MERGE_ALGORITHM = megType,$
   segmentation_raster_filename=segmentation_raster_filename, $
   report_filename=report_filename, $
   confidence_raster_image=confidence_raster_filename, $
   classification_raster_filename=classification_raster_filename, $
   vector_filename=vector_filename  
    end
  'Polynomial':begin
    envi_doit, 'envi_fx_examplebased_doit', fid=fid, pos=pos, dims=dims, $ 
   r_fid=r_fid, merge_level=merge,scale_level=segment, $
   classiication_algorithm=method,kernel_type = kernelType,kernel_bias = kernelBias,$
   CLASSIFICATION_THRESHOLD=threshold,penalty = kernerPanalty,$
   example_vector_filename=shapefile,segment_bands=pos, $
   segmentation_algorithm = segType,$
   segmentation_raster_filename=segmentation_raster_filename, $
   report_filename=report_filename, $
   confidence_raster_image=confidence_raster_filename, $
   classification_raster_filename=classification_raster_filename, $
   vector_filename=vector_filename  
    end
  'Sigmoid':begin
    envi_doit, 'envi_fx_examplebased_doit', fid=fid, pos=pos, dims=dims, $ 
   r_fid=r_fid, merge_level=merge,scale_level=segment, $
   classiication_algorithm=method,kernel_type = kernelType,penalty = kernerPanalty,$
   CLASSIFICATION_THRESHOLD=threshold,kernel_bias = kernelBias,$
   example_vector_filename=shapefile,segment_bands=pos, $
   segmentation_algorithm = segType,$
   segmentation_raster_filename=segmentation_raster_filename, $
   report_filename=report_filename, $
   confidence_raster_image=confidence_raster_filename, $
   classification_raster_filename=classification_raster_filename, $
   vector_filename=vector_filename  
    end
  endcase
  end
  'PCA':BEGIN
   envi_doit, 'envi_fx_examplebased_doit', fid=fid, pos=pos, dims=dims, $ 
   r_fid=r_fid, merge_level=merge,scale_level=segment, $
   classiication_algorithm=method,$
   CLASSIFICATION_THRESHOLD=threshold,$
   example_vector_filename=shapefile,segment_bands=pos, $
   segmentation_algorithm = segType,$
   segmentation_raster_filename=segmentation_raster_filename, $
   report_filename=report_filename, $
   confidence_raster_image=confidence_raster_filename, $
   classification_raster_filename=classification_raster_filename, $
   vector_filename=vector_filename  
  END
  
  endcase
;
; Exit ENVI
e.Close
;
; Re-open ENVI in interactive mode and display
; the classification image.
END

