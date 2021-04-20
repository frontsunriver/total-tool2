@if(!empty($menus))
	@php
	$mainMenus=json_decode($menus->data);
	@endphp
	@foreach ($mainMenus as $row) 
	<li  class="{{ $li }}" >
		@if($icon_position == 'left')<i class="{{ $row->icon }}"></i> @endif

		<a @if(url()->current() == url($row->href)) class="active" @endif href="{{ url($row->href) }}" @if(!empty($row->target)) target="{{ $row->target }}" @endif>{{ $row->text }}</a>

		@if($icon_position=='right') <i class="{{ $row->icon }}"></i>@endif
		@if (isset($row->children)) 
		<ul  class="{{ $ul }}" >
			@foreach($row->children as $childrens)
			 @include('lphelper::lphelper.lpmenu.child', ['childrens' => $childrens])
			@endforeach
		</ul>
		@endif
		</li>		
	@endforeach
@endif