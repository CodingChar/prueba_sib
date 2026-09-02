import { useState, type FormEvent } from 'react'
import { useNavigate } from 'react-router-dom'
import Layout from '../components/Layout'
import api from '../services/api'
import type { TipoEmpleado } from '../types/empleado'

const CAMPOS_COMUNES_ETIQUETA: Record<string, string> = {
  primerNombre: 'Primer nombre',
  apellidoPaterno: 'Apellido paterno',
  numeroSeguroSocial: 'Número de seguro social',
  departamento: 'Departamento',
  salarioSemanal: 'Salario semanal',
  sueldoPorHora: 'Sueldo por hora',
  horasTrabajadas: 'Horas trabajadas',
  ventasBrutas: 'Ventas brutas',
  tarifaComision: 'Tarifa de comisión (ej. 0.05 = 5%)',
  salarioBase: 'Salario base',
}

const CAMPOS_POR_TIPO: Record<TipoEmpleado, string[]> = {
  Asalariado: ['primerNombre', 'apellidoPaterno', 'numeroSeguroSocial', 'departamento', 'salarioSemanal'],
  PorHora: ['apellidoPaterno', 'numeroSeguroSocial', 'departamento', 'sueldoPorHora', 'horasTrabajadas'],
  PorComision: ['primerNombre', 'apellidoPaterno', 'numeroSeguroSocial', 'departamento', 'ventasBrutas', 'tarifaComision'],
  AsalariadoComision: ['primerNombre', 'apellidoPaterno', 'numeroSeguroSocial', 'departamento', 'ventasBrutas', 'tarifaComision', 'salarioBase'],
}

const ENDPOINT_POR_TIPO: Record<TipoEmpleado, string> = {
  Asalariado: '/empleados/asalariado',
  PorHora: '/empleados/por-hora',
  PorComision: '/empleados/por-comision',
  AsalariadoComision: '/empleados/asalariado-comision',
}

const CAMPOS_NUMERICOS = new Set([
  'salarioSemanal', 'sueldoPorHora', 'horasTrabajadas', 'ventasBrutas', 'tarifaComision', 'salarioBase',
])

function EmpleadosCrearPage() {
  const [tipo, setTipo] = useState<TipoEmpleado>('Asalariado')
  const [valores, setValores] = useState<Record<string, string>>({})
  const [error, setError] = useState<string | null>(null)
  const [enviando, setEnviando] = useState(false)
  const navigate = useNavigate()

  function cambiarTipo(nuevoTipo: TipoEmpleado) {
    setTipo(nuevoTipo)
    setValores({})
    setError(null)
  }

  function actualizarCampo(campo: string, valor: string) {
    setValores((anteriores) => ({ ...anteriores, [campo]: valor }))
  }

  async function manejarEnvio(evento: FormEvent) {
    evento.preventDefault()
    setError(null)
    setEnviando(true)

    const payload: Record<string, string | number | null> = {}
    for (const campo of CAMPOS_POR_TIPO[tipo]) {
      const valorCrudo = valores[campo] ?? ''
      if (CAMPOS_NUMERICOS.has(campo)) {
        payload[campo] = valorCrudo === '' ? 0 : Number(valorCrudo)
      } else if (campo === 'primerNombre') {
        payload[campo] = valorCrudo || null
      } else {
        payload[campo] = valorCrudo
      }
    }

    try {
      await api.post(ENDPOINT_POR_TIPO[tipo], payload)
      navigate('/empleados')
    } catch {
      setError('No se pudo crear el empleado. Verifica los datos.')
    } finally {
      setEnviando(false)
    }
  }

  return (
    <Layout titulo="Crear registro de empleado">
      <form onSubmit={manejarEnvio} className="max-w-lg">
        <label className="block text-sm font-medium text-gray-700 mb-1">Tipo de empleado</label>
        <select
          value={tipo}
          onChange={(e) => cambiarTipo(e.target.value as TipoEmpleado)}
          className="w-full border border-gray-300 rounded-md px-3 py-2 mb-5 text-sm"
        >
          <option value="Asalariado">Asalariado</option>
          <option value="PorHora">Por hora</option>
          <option value="PorComision">Por comisión</option>
          <option value="AsalariadoComision">Asalariado por comisión</option>
        </select>

        {CAMPOS_POR_TIPO[tipo].map((campo) => (
          <div key={campo} className="mb-4">
            <label className="block text-sm font-medium text-gray-700 mb-1">
              {CAMPOS_COMUNES_ETIQUETA[campo]}
            </label>
            <input
              type={CAMPOS_NUMERICOS.has(campo) ? 'number' : 'text'}
              step={CAMPOS_NUMERICOS.has(campo) ? '0.01' : undefined}
              value={valores[campo] ?? ''}
              onChange={(e) => actualizarCampo(campo, e.target.value)}
              required={campo !== 'primerNombre'}
              className="w-full border border-gray-300 rounded-md px-3 py-2 text-sm"
            />
          </div>
        ))}

        {error && <p className="text-red-600 text-sm mb-4">{error}</p>}

        <button
          type="submit"
          disabled={enviando}
          className="bg-[rgba(13,48,72,0.9)] text-white rounded-md px-5 py-2 text-sm font-medium disabled:opacity-50"
        >
          {enviando ? 'Guardando...' : 'Guardar empleado'}
        </button>
      </form>
    </Layout>
  )
}

export default EmpleadosCrearPage