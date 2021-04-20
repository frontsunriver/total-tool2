<script src="{{ asset('/js/jquery-3.5.1.min.js') }}"></script>
<script src="{{ asset('/js/popper.min.js') }}"></script>
<script src="{{ asset('/js/bootstrap.min.js') }}"></script>
<script src="{{ asset('/summernote/summernote-bs4.js') }}"></script>
<script>
$(document).ready(function () {
    $("#checkAll").change(function () {
    $("input:checkbox").prop('checked', $(this).prop("checked"));
    });
  	$('#summernote').summernote({
        height: 100,
        disableDragAndDrop: true,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
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
    $('#pagenote').summernote({
        height: 300,
        disableDragAndDrop: true,
        toolbar: [
            ['style', ['bold', 'italic', 'underline', 'clear']],
            ['mis', ['link']],
            ['fontsize', ['fontsize']],
            ['color', ['color']],
            ['para', ['ul', 'ol', 'paragraph']],
            ['img', ['picture']],
            ['other', ['undo', 'redo']],
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
});
</script>
