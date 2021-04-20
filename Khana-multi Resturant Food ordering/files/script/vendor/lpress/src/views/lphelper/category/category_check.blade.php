
@foreach ($categories as $category)
<?php
	if (in_array($category->id, $arr)) {
		$checked = "checked";
	}
	else{
		$checked = "";
	}
?>
<div class="custom-control custom-checkbox"><input  {{ $checked  }} type="checkbox" name="category[]" class="custom-control-input" value="{{ $category->id }}" id="category{{  $category->id  }}">
	<label class="custom-control-label" for="category{{ $category->id }}">{{ $category->name }}
	</label>
</div>
@foreach ($category->childrenCategories as  $childCategory)
@include('lphelper::lphelper.category.category_child_check', ['child_category' => $childCategory])
@endforeach


@endforeach
