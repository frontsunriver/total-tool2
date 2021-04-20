 @php
 $i=0;
 @endphp
 @foreach ($categories as $key => $category)
 <option value="{{ $category->id }}" @if($select == $category->id) selected @endif> {{ $category->name }}</option>

 @foreach ($category->childrenCategories as   $childCategory)

 @include('lphelper::lphelper.categoryconfig.child', ['child_category' => $childCategory])
 @endforeach

 @endforeach
