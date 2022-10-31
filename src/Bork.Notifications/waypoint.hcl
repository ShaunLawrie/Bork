project = "bork"

app "borknotifications" {
  build {
    use "docker" {
      dockerfile = "Bork.Notifications.dockerfile"
      context = ".."
    }
  }
  deploy {
    use "docker" { }
  }
}