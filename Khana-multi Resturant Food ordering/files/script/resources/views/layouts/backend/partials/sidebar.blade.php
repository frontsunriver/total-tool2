 <div class="main-sidebar">
  <aside id="sidebar-wrapper">
    <div class="sidebar-brand">
      <a href="#">{{ env('APP_NAME') }}</a>

    </div>
    <div class="sidebar-brand sidebar-brand-sm">
      <a href="#">{{ Str::limit(env('APP_NAME'), $limit = 1) }}</a>
    </div>
    <ul class="sidebar-menu">

     @foreach(AdminSidebar() as $key => $adminmenus)
     @if(isset($adminmenus['child']))
     @php
     $active  = $adminmenus['active'] ?? '';
     @endphp
     <li class="dropdown {{ $active ? 'active' : '' }}">
      <a href="#" class="nav-link has-dropdown" data-toggle="dropdown"><i class="{{ $adminmenus['icon'] ?? 'fas fa-columns' }}"></i> <span>{{ $adminmenus['name'] }}</span></a>
      <ul class="dropdown-menu">
        @foreach($adminmenus['child'] as $ch_key => $adminmenuChild)
        <li @if($adminmenuChild == url()->current()) class="active" @endif><a class="nav-link {{ $adminmenus['class'] ?? '' }}" href="{{ url($adminmenuChild) }}">{{ $ch_key }}</a></li>
        @endforeach
      </ul>
    </li>
    @else
   
    <li class="@if(url($adminmenus['url']) == url()->full()) active @endif">
      <a class="nav-link {{ $adminmenus['class'] ?? '' }}"  href="{{ url($adminmenus['url']) }}">
        <i class="{{ $adminmenus['icon'] ?? 'fas fa-columns' }}"></i> <span>{{ $adminmenus['name'] }}</span>
      </a>
    </li>
    @endif
    @endforeach
  </aside>
</div>


