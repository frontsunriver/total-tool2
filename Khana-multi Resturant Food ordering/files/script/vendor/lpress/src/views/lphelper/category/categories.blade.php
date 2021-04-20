@foreach ($categories as $category)
<div class="custom-control custom-checkbox"><input type="checkbox" name="category[]" class="custom-control-input" value="{{ $category->id }}" id="category{{  $category->id  }}">
	<label class="custom-control-label" for="category{{ $category->id }}">{{ $category->name }}
	</label>
</div>
@foreach ($category->childrenCategories as  $childCategory)
    @include('lphelper::lphelper.category.child_category', ['child_category' => $childCategory])
@endforeach
@endforeach
