import { NavLink } from 'react-router-dom'
import logoSb from '../assets/logo-sb.png'
import homeIcon from '../assets/icons/home.svg'

const navItems = [
  { to: '/empleados', label: 'Consulta' },
  { to: '/empleados/nuevo', label: 'Crear registro' },
]

function Sidebar() {
  return (
    <aside className="w-64 shrink-0 bg-[rgba(13,48,72,0.9)] text-white flex flex-col py-6">
      <div className="px-6 mb-8">
        <img src={logoSb} alt="Superintendencia de Bancos" className="w-32" />
      </div>

      <nav className="flex flex-col gap-1 px-3">
        <NavLink
          to="/"
          className={({ isActive }) =>
            `flex items-center gap-3 px-3 py-2 rounded-md text-sm font-medium transition-colors ${
              isActive ? 'bg-white/10' : 'hover:bg-white/5'
            }`
          }
        >
          <img src={homeIcon} alt="" className="w-5 h-5" />
          Inicio
        </NavLink>

        {navItems.map((item) => (
          <NavLink
            key={item.to}
            to={item.to}
            className={({ isActive }) =>
              `px-3 py-2 rounded-md text-sm font-medium transition-colors ${
                isActive ? 'bg-white/10' : 'hover:bg-white/5'
              }`
            }
          >
            {item.label}
          </NavLink>
        ))}
      </nav>
    </aside>
  )
}

export default Sidebar