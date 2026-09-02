import { useEffect, useState, type FormEvent } from 'react'
import { useNavigate, useParams } from 'react-router-dom'
import Layout from '../components/Layout'
import api from '../services/api'
import type { EmpleadoDetalle, TipoEmpleado } from '../types/empleado'

const CAMPOS_POR_TIPO: Record<TipoEmpleado, string[]> = {
  Asalariado: ['salarioSemanal'],
  PorHora: ['sueldoPorHora', 'horasTrabajadas'],
  PorComision: ['ventasBrutas', 'tarifaComision'],
  AsalariadoComision: ['ventasBrutas', 'tarifaComision', 'salarioBase'],
}

const ETIQUETAS: Record<string, string> = {
  salarioSemanal: 'Salario semanal',
  sueldoPorHora: 'Sueldo por hora',
  horasTrabajadas: 'Horas trabajadas',
  ventasBrutas: 'Ventas brutas',
  tarifaComision: 'Tarifa de comisión (ej. 0.05 = 5%)',
  salarioBase: 'Salario base',
}

const ENDPOINT_POR_TIPO: Record<TipoEmpleado, string> = {
  Asalariado: 'asalariado',
  PorHora: 'por-hora',
  PorComision: 'por-comision',
  AsalariadoComision: 'asalariado-comision',
}

function EmpleadosEditarPage() {
  const { id } = useParams<{ id: string }>()
  const navigate = useNavigate()

  const [detalle, setDetalle] = useState<EmpleadoDetalle | null>(null)
  const [valores, setValores] = useState<Record<string, string>>({})
  const [cargando, setCargando] = useState(true)
  const [guardando, setGuardando] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    api.get<EmpleadoDetalle>(`/empleados/${id}`)
      .then((respuesta) => {
        const datos = respuesta.data
        setDetalle(datos)

        const inicial: Record<string, string> = {}
        for (const campo of CAMPOS_POR_TIPO[datos.tipo]) {
          const valorActual = (datos as unknown as Record<string, number | null>)[campo]
          inicial[campo] = valorActual !== null && valorActual !== undefined ? String(valorActual) : ''
        }
        setValores(inicial)
      })
      .catch(() => setError('No se pudo cargar el empleado.'))
      .finally(() => setCargando(false))
  }, [id])

  function actualizarCampo(campo: string, valor: string) {
    setValores((anteriores) => ({ ...anteriores, [campo]: valor }))
  }

  async function manejarEnvio(evento: FormEvent) {
    evento.preventDefault()
    if (!detalle) return

    setError(null)
    setGuardando(true)

    const payload: Record<string, number> = {}
    for (const campo of CAMPOS_POR_TIPO[detalle.tipo]) {
      payload[campo] = Number(valores[campo] ?? 0)
    }

    try {
      await api.put(`/empleados/${ENDPOINT_POR_TIPO[detalle.tipo]}/${detalle.id}`, payload)
      navigate('/empleados')
    } catch {
      setError('No se pudo actualizar el empleado.')
    } finally {
      setGuardando(false)
    }
  }

  if (cargando) {
    return (
      <Layout titulo="Editar empleado">
        <p className="text-gray-500 text-sm">Cargando...</p>
      </Layout>
    )
  }

  if (!detalle) {
    return (
      <Layout titulo="Editar empleado">
        <p className="text-red-600 text-sm">{error ?? 'Empleado no encontrado.'}</p>
      </Layout>
    )
  }

  return (
    <Layout titulo={`Editar empleado: ${detalle.primerNombre ?? ''} ${detalle.apellidoPaterno}`.trim()}>
      <form onSubmit={manejarEnvio} className="max-w-lg">
        <p className="text-sm text-gray-500 mb-5">
          Tipo: <span className="font-medium text-gray-700">{detalle.tipo}</span>
          {' · '}
          Departamento: <span className="font-medium text-gray-700">{detalle.departamento}</span>
        </p>

        {CAMPOS_POR_TIPO[detalle.tipo].map((campo) => (
          <div key={campo} className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              {ETIQUETAS[campo]}
            </label>
            <input
              type="number"
              step="0.01"
              value={valores[campo] ?? ''}
              onChange={(e) => actualizarCampo(campo, e.target.value)}
              required
              className="w-full border border-gray-300 rounded-md px-3 py-2 text-sm"
            />
          </div>
        ))}

        {error && <p className="text-red-600 text-sm mb-4">{error}</p>}

        <button
          type="submit"
          disabled={guardando}
          className="bg-[rgba(13,48,72,0.9)] text-white rounded-md px-5 py-2 text-sm font-medium disabled:opacity-50"
        >
          {guardando ? 'Guardando...' : 'Guardar cambios'}
        </button>
      </form>
    </Layout>
  )
}

export default EmpleadosEditarPage