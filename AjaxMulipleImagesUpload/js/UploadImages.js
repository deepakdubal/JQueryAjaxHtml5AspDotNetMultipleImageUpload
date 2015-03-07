var fileinput = document.getElementById('myFile0');
var max_width = fileinput.getAttribute('data-maxwidth');
var max_height = fileinput.getAttribute('data-maxheight');
var max_widthUpload = fileinput.getAttribute('data-maxwidthUpload');
var max_heightUpload = fileinput.getAttribute('data-maxheightUpload');
var preview = document.getElementById('Preview');
var form = document.getElementById('form');
function processfile(file) {

    if (!(/image/i).test(file.type)) {
        alert("File " + file.name + " is not an image.");
        $('#myFile0').val("");
        return false;
    }

    // read the files
    var reader = new FileReader();
    reader.readAsArrayBuffer(file);

    reader.onload = function (event) {
        // blob stuff
        var blob = new Blob([event.target.result]); // create blob...
        window.URL = window.URL || window.webkitURL;
        var blobURL = window.URL.createObjectURL(blob); // and get it's URL

        // helper Image object
        var image = new Image();
        image.src = blobURL;
        //preview.appendChild(image); // preview commented out, I am using the canvas instead
        image.onload = function () {
            // have to wait till it's loaded
            var resized = resizeMe(image); // send it to canvas
            var newinput = document.createElement("input");
            var d = new Date();
            newinput.type = 'hidden';
            newinput.name = 'images' + d.getMilliseconds() ;
            newinput.value = resized; // put result from canvas into new hidden input
            document.getElementById('Preview').appendChild(newinput);
        }
    };
}

function readfiles(files) {
 
    // remove the existing canvases and hidden inputs if user re-selects new pics
    var existinginputs = document.getElementsByName('images[]');
    var existingcanvases = document.getElementsByTagName('canvas');
    while (existinginputs.length > 0) { // it's a live list so removing the first element each time
        // DOMNode.prototype.remove = function() {this.parentNode.removeChild(this);}
        form.removeChild(existinginputs[0]);
        preview.removeChild(existingcanvases[0]);
    }

    for (var i = 0; i < files.length; i++) {
        processfile(files[i]); // process each file at once
    }
    //fileinput.value = ""; //remove the original files from fileinput
    // TODO remove the previous hidden inputs if user selects other files
}

// this is where it starts. event triggered when user selects files
fileinput.onchange = function () {
    if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
        alert('The File APIs are not fully supported in this browser.');
        return false;
    }
    //$("#Button2").show();
    if (fileinput.files.length > 50) {
        alert("Please Select maximum 50 images.")
        $('#myFile0').val("");
        return false;
    } else {
        readfiles(fileinput.files);
        $('#Button1').show();
    }
    
}

// === RESIZE ====

function resizeMe(img) {

    var canvas = document.createElement('canvas');
    var width = img.width;
    var height = img.height;

    // calculate the width and height, constraining the proportions
    if (width > height) {
        if (width > max_width) {
            //height *= max_width / width;
            height = Math.round(height *= max_width / width);
            width = max_width;
        }
    } else {
        if (height > max_height) {
            //width *= max_height / height;
            width = Math.round(width *= max_height / height);
            height = max_height;
        }
    }

    // resize the canvas and draw the image data into it
    canvas.width = width;
    canvas.height = height;
    var ctx = canvas.getContext("2d");
    ctx.drawImage(img, 0, 0, width, height);
    preview.appendChild(canvas); // do the actual resized preview

    var canvasUpload = document.createElement('canvas');
    var width = img.width;
    var height = img.height;

    // calculate the width and height, constraining the proportions
    if (width > height) {
        if (width > max_widthUpload) {
            //height *= max_width / width;
            height = Math.round(height *= max_widthUpload / width);
            width = max_widthUpload;
        }
    } else {
        if (height > max_heightUpload) {
            //width *= max_height / height;
            width = Math.round(width *= max_heightUpload / height);
            height = max_heightUpload;
        }
    }

    // resize the canvas and draw the image data into it
    canvasUpload.width = width;
    canvasUpload.height = height;
   // canvasUpload.setAttribute("class", "upload");
    var ctx2 = canvasUpload.getContext("2d");
    ctx2.drawImage(img, 0, 0, width, height);
    //preview.appendChild(canvasUpload); // do the actual resized preview
    return canvasUpload.toDataURL("image/jpeg", 0.7); // get the data from canvas as 70% JPG (can be also PNG, etc.)

}

$(document).ready(function () {
    if (!(window.File && window.FileReader && window.FileList && window.Blob)) {
        
        $("#MainDiv").hide();
        $("#MessageDiv").show();
  
    } else {
        $("#MainDiv").show();
        $("#MessageDiv").hide();
    }


    var globalCount = 0;
    $("#Button2").click(function () {
        $("#Button1").hide();
        upload(globalCount);
     
    });


    function upload(cc) {
        var allInput = document.getElementById('Preview').getElementsByTagName('input');
        if (globalCount <= (allInput.length-1)) {
            dataURL = allInput[cc].value.replace('data:image/jpeg;base64,', '');
            $('#loading').css({ top: '50%', left: '50%', margin: '-' + ($('#loading').outerHeight() / 2) + 'px 0 0 -' + ($('#loading').outerWidth() / 2) + 'px' });
            $(".loder").html("Please wait your images(" + globalCount + "/ " + allInput.length + ") is being uploaded").show(function () {
              $('#loading').css({ top: '50%', left: '50%', margin: '-' + ($('#loading').outerHeight() / 2) + 'px 0 0 -' + ($('#loading').outerWidth() / 2) + 'px' });
            });
            $("#UpdateProgress1").fadeIn();
            $.ajax({
                type: "POST",
                url: "UploadImage.aspx/SaveImage",
                data: "{ 'image':'" + dataURL + "','Acode':'" + $("#FolderName").val() + "'}",
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (d) {
                    globalCount++;
                    upload(globalCount);
                },
                always: function() {
                    globalCount++;
                        },
                error: function (e, t) {
                    alert(e.responseText)
                    $("#UpdateProgress1").fadeOut();
                }
                
            });
        } else {
            $('#myFile0').val("");
            $(".loder").hide();
            $(".success").show();
            $("#Button3").click();
        }
    }
});


function form_Submit()
{

    if (document.getElementById("myFile0").validity.valid) {
        $('#Button1').hide();
        $("#Button2").click();
        return false;
    } else {
        return false;
    }

}