type TopBarProps = {
  titulo: string
}

function TopBar({ titulo }: TopBarProps) {
  return (
    <header className="bg-[rgba(13,48,72,0.9)] px-8 py-6">
      <h1 className="text-white text-2xl font-semibold">{titulo}</h1>
    </header>
  )
}

export default TopBar