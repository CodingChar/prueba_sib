export type TipoEmpleado = 'Asalariado' | 'PorHora' | 'PorComision' | 'AsalariadoComision'

export type EmpleadoResponse = {
  id: number
  tipo: TipoEmpleado
  primerNombre: string | null
  apellidoPaterno: string
  numeroSeguroSocial: string
  departamento: string
  estado: string
  pagoCalculado: number
}

export type CrearAsalariadoPayload = {
  primerNombre: string | null
  apellidoPaterno: string
  numeroSeguroSocial: string
  departamento: string
  salarioSemanal: number
}

export type CrearPorHoraPayload = {
  apellidoPaterno: string
  numeroSeguroSocial: string
  departamento: string
  sueldoPorHora: number
  horasTrabajadas: number
}

export type CrearPorComisionPayload = {
  primerNombre: string | null
  apellidoPaterno: string
  numeroSeguroSocial: string
  departamento: string
  ventasBrutas: number
  tarifaComision: number
}

export type CrearAsalariadoComisionPayload = {
  primerNombre: string | null
  apellidoPaterno: string
  numeroSeguroSocial: string
  departamento: string
  ventasBrutas: number
  tarifaComision: number
  salarioBase: number
}