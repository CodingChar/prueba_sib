export type EntidadGubernamental = {
  id: number
  nombre: string
  categoria: string
  poderDelEstado: string
  sector: string
}

export type CrearEntidadGubernamentalPayload = {
  nombre: string
  categoria: string
  poderDelEstado: string
  sector: string
}