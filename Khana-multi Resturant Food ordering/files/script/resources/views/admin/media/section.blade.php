<div class="single-area">
 <div class="card sub">
  <div class="card-body">
   <h5><a href="#" data-toggle="modal" data-target=".media-single" class="text-dark">{{ $title }}</a></h5>
   <hr>
   <a href="#" data-toggle="modal" data-target=".media-single" class="single-modal">
   
    <img class="img-fluid" id="{{ $preview_id }}" src="{{ asset($preview) }}"></a>
  </div>
</div>
</div>
<input type="hidden" id="{{ $input_id }}" class="{{ $input_class }}" name="{{ $input_name }}" value="{{ $value }}">