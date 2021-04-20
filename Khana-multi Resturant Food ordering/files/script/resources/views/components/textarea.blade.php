 <div class="form-group">
 	<label for="{{ $id }}">{{ $title }}</label>
 	<textarea name="{{ $name }}" class="form-control {{ $class }}" cols="{{ $cols }}" rows="{{ $rows }}" placeholder="{{ $placeholder }}" id="{{ $id }}" maxlength="{{ $maxlength }}" @if($is_required==true) required="" @endif>{{ $value }}</textarea>
 </div>