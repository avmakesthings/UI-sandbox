New VR Panorama has introduced optional stereo panorama rendering mode. WIth this mode you can have a perfect stitching. As this mode has to use multi-pass rendering, it doesn't have a same performance as a standard VR Panorama stereo rendering. 
Usage: 
VR Panorama defaults to classic VR Panorama rendering mode. To activate new stitchless rendering, go to Optimizations and select HQ Stereo Stitch checkbox. 

HQ Stereo Stitch will give you another few options: 
-Render Slices; This option controls how many slices (passes) should be rendered. Higher values gives better quality stitching, but also slows down rendering as each slice requires scene to be rendered again. 
-Smoothing; this value controls how smoothly slices get blended. You can use it to optimize rendering with low number of slices at a cost of blurring objects close to a camera.



IMPORTANT: When using Hq mode, set Environment distance to something high like 10 000. But you can experiment also with standard (close) values. 