$(document).ready(function(){
    // Ajax Header //
    $.ajaxSetup({
        headers: {
            "X-CSRF-TOKEN": $("meta[name='csrf-token']").attr("content")
        }
    });
    // Add new field Function //
    $(".btnadd").click(function(){ 
        var template = $(this).attr("name");
        var html = $("#" +template).html();
        $("#dynamic_field").append(html);
        $("html, body").animate({ scrollTop: $("#subbtn").offset().top }, "slow");
        return false;
    });

    // Remove field Function //
    $(document).on("click", ".btn_remove", function(){ 
        $(this).parents(".form-group").remove();
    });

    // Add new text editor field Function //
    var i=1;
    $('#texteditor').click(function(){
       i++;  
       $('#dynamic_field').append('<div id="row'+i+'" class="form-group row"><label class="offset-md-1 col-md-2 col-form-label">' + editortxt + '</label><div class="col-sm-10 col-md-7"><textarea id="summer'+i+'" name="content[]"></textarea><input type="hidden" name="type[]" value="txteditor"></div><div class="col-sm-2 col-md-1"><button id="'+i+'" type="button" class="btn btn-danger remove_editor">X</button></div></div>');
       $('#summer'+i+'').summernote({
        height: 100,
        disableDragAndDrop: true,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['para', ['ul', 'ol']],
            ['mis', ['link']],   
        ],
        callbacks: {
            onPaste: function (e) {
                var bufferText = ((e.originalEvent || e).clipboardData || window.clipboardData).getData('Text');
                e.preventDefault();
                setTimeout( function(){
                    document.execCommand( 'insertText', false, bufferText );
            }, 10 );
        }
        }
        });
        $("html, body").animate({ scrollTop: $("#subbtn").offset().top }, "slow");
        return false;
    });

    // Remove text editor field Function //
    $(document).on('click', '.remove_editor', function(){
        var button_id = $(this).attr("id");
        $('#summer'+button_id+'').summernote('destroy');   
        $('#row'+button_id+'').remove();
    });

    // Delete Content Function //
    $(document).on("click", ".btn_del", function(){
        $(this).parents(".form-group").remove();
        var contentId = $(this).closest(".form-group").find("input[name='id[]']").val();
        $.ajax({            
            url: delContent,
            type: "POST",
            data: { id : contentId },
            dataType: "json",
            }).done(function(response){
                if(response.error){
                    $(formInfo).find(".print-error-msg").html("").append("<p>" + response.error + "</p>");
                }
        });
    });

    // Post embed Function //
    $(document).on("click", ".embed_btn", function(e){
        var formGroup = $(this).closest(".form-group");
        var inputGroup = $(this).closest(".input-group");
        var embedForm = $(inputGroup).find("input[name='embed_url']").val();
        var embedType = $(formGroup).find("input[name='type[]']").val();
        e.preventDefault();
        $.ajax({
            url : embedURL,
            type : 'POST',
            data : { embed : embedForm, type : embedType  },
            dataType: "json",
            beforeSend: function() {
                $(inputGroup).find("input[name='embed_url']").val(processing);
            },
        }).done(function(response){
            if(response.success){
                $(formGroup).find(".text-danger").remove();
                $(formGroup).find("input[name='content[]']").val(response.success);
                $(inputGroup).find("input[name='embed_url']").prop("readonly", true).val(embedForm);
                $(inputGroup).find("input[name='embed_url']").after("<div class='input-group-addon'><i class='icon-check icons text-success'></div>");
                $(inputGroup).find(".embed_btn").remove();
                $(inputGroup).find(".input-group-btn").append("<button type='button' class='btn btn-warning remove_embed'>" + removetxt + "</button>");
            } else {
                $(inputGroup).find("input[name='embed_url']").val(embedForm);
                $(formGroup).find(".text-danger").remove();
                $(formGroup).append("<small class='offset-md-3 col-md-8 col-form-label text-danger'>" + response.error + "</small>");
            }
        });
    });

    // Remove embed Function //
    $(document).on("click", ".remove_embed", function(){
        var formGroup = $(this).closest(".form-group");
        var inputGroup = $(this).closest(".input-group");
        $(formGroup).find("input[name='content[]']").val("");
        $(inputGroup).find("input[name='embed_url']").prop("readonly", false).val("");
        $(inputGroup).find(".input-group-addon").remove();
        $(inputGroup).find(".remove_embed").remove();
        $(inputGroup).find(".input-group-btn").append("<button type='button' class='btn btn-success embed_btn'>" + embedtxt + "</button>");
    });

    // Post submit Function //
    $("#submit").click(function(e){
        var form = $('#post_form');
        e.preventDefault();
        $.ajax({
            url     : form.attr("action"),
            type    : form.attr("method"),
            data    : form.serialize(),
            dataType: "json",
            beforeSend: function() {
                $('#submit').removeClass("btn-success");
                $('#submit').addClass("btn-secondary").val(processing);
                $('#submit').prop( "disabled", true );
            },
        }).done(function(response){
            if(response.success){
                $("#post_form")[0].reset();
                $("#post_form").remove();
                $(".print-success-msg").html("").removeClass("d-none");
                $(".print-error-msg").addClass("d-none");
                $(".print-success-msg").append(response.success);
            } else {
                $(".print-error-msg").find("ul").html("");
                $(".print-error-msg").removeClass("d-none");
                $(".print-success-msg").addClass("d-none");
                $("#submit").removeClass("btn-secondary").addClass("btn-success").val(submittxt);
                $("#submit").prop( "disabled", false );
                $.each( response.error, function( key, value ) {
                    $(".print-error-msg").find("ul").append("<li>"+value+"</li>");
                });           
            }
        });
    });
    // Image Upload Function //
    $(document).on("change", ".fileupload", function () {
        var file = $(this)[0].files[0];
        var formInfo = $(this).closest(".form-group");
        var formData = new FormData();
        formData.append("post_image", file);  
        $.ajax({
            url : imgURL,
            type: "POST",         
            dataType: "json",
            data : formData,
            contentType: false,
            cache: false,
            processData:false,
            beforeSend: function() {
                $(formInfo).find(".fileinfo").html("").append("<p>" + fileUploading + "</p>");
            },
        }).done(function(response){
            if(response.success){
                $(formInfo).find("input[class='photo_upload']").val(response.success);
                $(formInfo).find(".fileinfo").html("").append("<p><img class='imgthumb img-fluid' src='" + avatarURL + "/" + response.success + "'>" + imguploaded + "</p>");
                $(formInfo).find(".btn_remove").addClass("d-none");
                $(formInfo).find(".fileinputs").html("").append("<button type='button' class='btn btn-warning btn-block removeimg'>" + removetxt + "</button>");
            } else {
                $(formInfo).find(".fileinfo").html("").append("<p>" + response.error[0] + "</p>");
            }
        });
    });
    // Image Delete Function //
    $(document).on('click', '.removeimg', function () {
        var formInfo = $(this).closest(".form-group");
        var fileName = $(formInfo).find("input[class='photo_upload']").val();
         $.ajax({            
            url: delURL,
            type: "POST",
            data: { id : fileName },
            dataType: "json",
            }).done(function(response){
                if(response.success){
                    $(formInfo).find("input[class='photo_upload']").val("");
                    $(formInfo).find(".fileinfo").html("").append("<p>" + response.success + "</p>");
                    $(formInfo).find(".fileinputs").html("").append("<label class='btn btn-info btn-block btnfile'>" + browse + "<input class='fileupload d-none' type='file' name='post_image'></label>");
                    $(formInfo).find(".btn_remove").removeClass("d-none");
                } else {
                    $(formInfo).find(".fileinfo").html("").append("<p>" + response.error + "</p>");
                }
        }); 
    });
    // Image remove on edit //
    $(document).on('click', '.editimage', function () {
        var formInfo = $(this).closest(".form-group");
        $(formInfo).find("input[class='photo_upload']").val("");
        $(formInfo).find(".fileinfo").html("").append("<p>" + imgremoved + "</p>");
        $(formInfo).find(".fileinputs").html("").append("<label class='btn btn-info btn-block btnfile'>" + browse + "<input class='fileupload d-none' type='file' name='post_image'></label>");
    });
});
