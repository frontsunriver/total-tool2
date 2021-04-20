<div class="form-group">
	<label for="{{ $id }}">{{ $title }}</label>
	<input type="{{ $type }}" placeholder="{{ $placeholder }}" name="{{ $name }}" class="form-control" id="{{ $id }}" @if($required == true) required @endif value="{{ $value }}" autocomplete="off">
</div>