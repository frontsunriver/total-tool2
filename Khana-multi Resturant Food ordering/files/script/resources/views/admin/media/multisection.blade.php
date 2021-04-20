<div class="single-area">
	<div class="card sub">
		<div class="card-body">
			<h5><a href="#" data-toggle="modal" data-target=".media-multiple" class="multi-modal">
			{{ $title }}</a></h5>
			<hr>
			<a href="#" data-toggle="modal" data-target=".media-multiple" class="single-modal">
					
				@if(empty($preview))	
				<img class="img-fluid" id="{{ $preview_id }}" src="{{ asset('admin/img/img/placeholder.png') }}">
				@else
				
				@foreach($preview ?? [] as $row)
				<img class="gallary-src" height="80" src="{{ asset($row) }}"/>
				@endforeach
				@endif
				<div id="{{ $area_id }}"></div>
			</a>
		</div>
	</div>
</div>
<input type="hidden" id="{{ $input_id }}" class="{{ $input_class }}" name="{{ $input_name }}" value="{{ $value }}">