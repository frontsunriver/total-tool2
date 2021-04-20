<div class="custom-control custom-checkbox"><input type="checkbox" name="category[]" class="custom-control-input" value="{{ $child_category->id }}" id="category{{  $child_category->id  }}">
	<label class='custom-control-label' for="category{{ $child_category->id }}">{{ $child_category->name }}
	</label>
	@if ($child_category->categories)
  
        @foreach ($child_category->categories as $childCategory)
            @include('lphelper::lphelper.category.child_category', ['child_category' => $childCategory])
        @endforeach
   
	@endif
</div>
