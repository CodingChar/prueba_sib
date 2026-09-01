import type { ReactNode } from 'react'
import Sidebar from './Sidebar'
import TopBar from './TopBar'

type LayoutProps = {
  titulo: string
  children: ReactNode
}

function Layout({ titulo, children }: LayoutProps) {
  return (
    <div className="min-h-screen flex bg-[rgba(237,240,247,1)]">
      <Sidebar />
      <div className="flex-1 flex flex-col">
        <TopBar titulo={titulo} />
        <main className="p-8">
          <div className="bg-white rounded-2xl shadow-sm p-8 min-h-[400px]">
            {children}
          </div>
        </main>
      </div>
    </div>
  )
}

export default Layout