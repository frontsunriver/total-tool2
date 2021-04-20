<div class="section-header">
  <h1>{{ $title }}</h1>
  <div class="section-header-breadcrumb">
  	  @foreach(request()->segments() as $segment)
      <div class="breadcrumb-item">{{ $segment }}</div>
      @endforeach
      
  </div>
</div>