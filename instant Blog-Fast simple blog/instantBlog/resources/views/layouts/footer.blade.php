<script src="{{ asset('/js/jquery-3.5.1.min.js') }}"></script>
<script src="{{ asset('/js/salvattore.min.js') }}"></script>
<script src="{{ asset('/js/popper.min.js') }}"></script>
<script src="{{ asset('/js/bootstrap.min.js') }}"></script>
<script src="{{ asset('/js/heart.js') }}"></script>
<script type="text/javascript">
  $(document).ready(function() {
  	$('[data-toggle="tooltip"]').tooltip({
        trigger : 'hover',
        container: 'body'
    });
    $(".se-pre-con").fadeOut("slow");
  });
</script>