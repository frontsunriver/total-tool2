<div class="single-area">
	<div class="card sub">
		<div class="card-body">
			<h5>{{ __('Language') }}</h5>
			<hr>
			<select class="custom-select mr-sm-2" id="inlineFormCustomSelect" name="lang">
				@foreach($data as $k => $r)
				<option value="{{ $r }}" @if($r==$c) selected @endif>{{ $k }}</option>
				@endforeach
			</select>
		</div>
	</div>
</div>