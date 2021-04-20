@php
$i++
@endphp
<option @if($select == $child_category->id) selected @endif value="{{ $child_category->id }}" >@for($j=0; $j < $i ; $j++) &nbsp; @endfor 
 {{ $child_category->name }}</option>
@if ($child_category->categories)
        @foreach ($child_category->categories as $key => $childCategory)
            @include('lphelper::lphelper.categoryconfig.child', ['child_category' => $childCategory,'key'=>$key])
        @endforeach
@endif
