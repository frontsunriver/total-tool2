<nav class="navbar navbar-expand-md navbar-dark fixed-top bg-dark">
    <button class="navbar-toggler navbar-toggler-right" type="button" data-toggle="collapse" data-target="#navbarsExampleDefault" aria-controls="navbarsExampleDefault" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
    </button>
    <a class="navbar-brand ml-md-5" href="{{ url('/admin') }}">ADMIN</a>
    <div class="collapse navbar-collapse" id="navbarsExampleDefault">
    <ul class="navbar-nav ml-auto mr-md-5">
      <li class="nav-item {{ Request::is('admin') ? 'active' : '' }}">
            <a class="nav-link mr-2" href="{{ url('/admin') }}"><i class="icon-home icons"></i> Dashboard</a>
        </li>
        <li class="nav-item {{ Request::is('contents') ? 'active' : '' }}">
            <a class="nav-link mr-2" href="{{ url('/contents') }}"><i class="icon-layers icons"></i> Posts</a>
        </li>
        <li class="nav-item {{ Request::is('pages') ? 'active' : '' }}">
            <a class="nav-link mr-2" href="{{ url('/pages') }}"><i class="icon-docs icons"></i> Pages</a>
        </li>
        <li class="nav-item {{ Request::is('cats') ? 'active' : '' }}">
            <a class="nav-link mr-2" href="{{ url('/cats') }}"><i class="icon-grid icons"></i> Categories</a>
        </li>
        <li class="nav-item {{ Request::is('settings') ? 'active' : '' }}">
            <a class="nav-link mr-2" href="{{ url('/settings') }}"><i class="icon-settings icons"></i> Settings</a>
        </li>
        @can('admin-secret')
        <li class="nav-item {{ Request::is('adminprofile') ? 'active' : '' }}">
            <a class="nav-link mr-2" href="{{ url('/adminprofile') }}"><i class="icon-user icons"></i> Profile</a>
        </li>
        @endcan
        <li class="nav-item {{ Request::is('users') ? 'active' : '' }}">
            <a class="nav-link mr-2" href="{{ url('/users') }}"><i class="icon-people icons"></i> Users</a>
        </li>
        <li class="nav-item">
            <a class="nav-link mr-2" target="_blank" href="{{ url('/') }}"><i class="icon-screen-desktop icons"></i> View Website</a>
        </li>
        <li class="nav-item">
            <a class="nav-link mr-2" href="{{ url('/logout') }}"
            onclick="event.preventDefault();
            document.getElementById('logout-form').submit();"><i class="icon-logout icons"></i> Logout
            </a>
        </li>
        <form id="logout-form" action="{{ url('/logout') }}" method="POST" style="display: none;">
          @csrf
        </form>
    </ul>
    </div>
</nav>