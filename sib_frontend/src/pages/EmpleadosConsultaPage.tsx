import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import Layout from '../components/Layout'
import api from '../services/api'
import type { EmpleadoResponse } from '../types/empleado'

function EmpleadosConsultaPage() {
  const [empleados, setEmpleados] = useState<EmpleadoResponse[]>([])
  const [nombre, setNombre] = useState('')
  const [departamento, setDepartamento] = useState('')
  const [estado, setEstado] = useState('')
  const [cargando, setCargando] = useState(false)

  async function buscar() {
    setCargando(true)
    try {
      const respuesta = await api.get<EmpleadoResponse[]>('/empleados', {
        params: {
          nombre: nombre || undefined,
          departamento: departamento || undefined,
          estado: estado || undefined,
        },
      })
      setEmpleados(respuesta.data)
    } finally {
      setCargando(false)
    }
  }

  useEffect(() => {
    buscar()
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [])

  return (
    <Layout titulo="Consulta de Empleados">
      <div className="flex flex-wrap gap-3 mb-6">
        <input
          type="text"
          placeholder="Nombre o apellido"
          value={nombre}
          onChange={(e) => setNombre(e.target.value)}
          className="border border-gray-300 rounded-md px-3 py-2 text-sm flex-1 min-w-[180px]"
        />
        <input
          type="text"
          placeholder="Departamento"
          value={departamento}
          onChange={(e) => setDepartamento(e.target.value)}
          className="border border-gray-300 rounded-md px-3 py-2 text-sm flex-1 min-w-[180px]"
        />
        <select
          value={estado}
          onChange={(e) => setEstado(e.target.value)}
          className="border border-gray-300 rounded-md px-3 py-2 text-sm"
        >
          <option value="">Todos los estados</option>
          <option value="Activo">Activo</option>
          <option value="Inactivo">Inactivo</option>
        </select>
        <button
          onClick={buscar}
          className="bg-[rgba(13,48,72,0.9)] text-white rounded-md px-4 py-2 text-sm font-medium"
        >
          Buscar
        </button>
      </div>

      {cargando ? (
        <p className="text-gray-500 text-sm">Cargando...</p>
      ) : (
        <div className="overflow-x-auto">
          <table className="w-full text-sm text-left">
            <thead>
              <tr className="border-b border-gray-200 text-gray-500">
                <th className="py-2 pr-4">Nombre</th>
                <th className="py-2 pr-4">Apellido</th>
                <th className="py-2 pr-4">Tipo</th>
                <th className="py-2 pr-4">Departamento</th>
                <th className="py-2 pr-4">Estado</th>
                <th className="py-2 pr-4 text-right">Pago Calculado</th>
                <th className="py-2 pr-4"></th>
              </tr>
            </thead>
            <tbody>
              {empleados.map((empleado) => (
                <tr key={empleado.id} className="border-b border-gray-100">
                  <td className="py-2 pr-4">{empleado.primerNombre ?? '—'}</td>
                  <td className="py-2 pr-4">{empleado.apellidoPaterno}</td>
                  <td className="py-2 pr-4">{empleado.tipo}</td>
                  <td className="py-2 pr-4">{empleado.departamento}</td>
                  <td className="py-2 pr-4">{empleado.estado}</td>
                  <td className="py-2 pr-4 text-right">
                    RD$ {empleado.pagoCalculado.toLocaleString('es-DO', { minimumFractionDigits: 2 })}
                  </td>
                  <td className="py-2 pr-4 text-right">
                    <Link
                      to={`/empleados/${empleado.id}/editar`}
                      className="text-blue-700 text-xs font-medium hover:underline"
                    >
                      Editar
                    </Link>
                  </td>
                </tr>
              ))}
              {empleados.length === 0 && (
                <tr>
                  <td colSpan={7} className="py-6 text-center text-gray-400">
                    No se encontraron empleados.
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      )}
    </Layout>
  )
}

export default EmpleadosConsultaPage