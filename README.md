# MoveMouse

Utilidad para Windows que envia movimiento relativo continuo del mouse hacia
la derecha a 120 unidades por segundo. Usa `SendInput` y permite detener el
movimiento con **Espacio**, incluso cuando otra ventana tiene el foco.

Este es un proyecto independiente: no esta afiliado al proyecto
[`sw3103/movemouse`](https://github.com/sw3103/movemouse).

## Uso

1. Abre `MoveMouse.exe` desde una publicacion de confianza.
2. Pulsa **Iniciar**. Puedes minimizar la ventana.
3. Pulsa **Espacio**, **Detener** o cierra la ventana para detenerlo.

El movimiento continua aunque el cursor alcance un borde de la pantalla.
Espacio solo detiene el movimiento: para reanudarlo, pulsa Iniciar.
Si no se puede registrar el atajo global, el programa no inicia el movimiento.

No requiere permisos de administrador, no instala servicios, no se agrega
al inicio de Windows y no necesita conexion a Internet. Para desinstalarlo,
cierra la aplicacion y elimina su carpeta.

### Limites y uso responsable

- El movimiento puede interferir con otras aplicaciones.
- Los juegos y sistemas anticheat pueden ignorar la entrada simulada.
- No garantiza mover la camara ni evitar comprobaciones de inactividad.
- No inyecta codigo ni intenta eludir protecciones o sistemas anticheat.
- Respeta las reglas de las aplicaciones y juegos donde lo utilices.

## Compilar

Requiere Windows de 64 bits con .NET Framework 4.x. No necesita NuGet ni SDKs
adicionales. En PowerShell, desde la carpeta del proyecto:

```powershell
.\build.ps1
```

El resultado se guarda en `dist/`, con instrucciones, licencia y un hash SHA-256.
No reemplaza el `MoveMouse.exe` original de la carpeta principal.
Los hashes sirven para comprobar integridad; **no sustituyen una firma digital**.
Si Windows bloquea el script o el ejecutable, no desactives sus protecciones.
Si PowerShell indica que la ejecucion de scripts esta deshabilitada, utiliza
la compilacion de GitHub Actions sin cambiar la politica local.

GitHub Actions tambien compila el proyecto en un runner hospedado por GitHub
mediante [build.yml](.github/workflows/build.yml). El artefacto
`MoveMouse-unsigned` **no esta firmado** y puede ser bloqueado por Windows.
Para cambiar la version, actualiza ambos atributos de version en `AssemblyInfo.cs`.

## Code signing policy

**Estado: preparacion para solicitar SignPath; sin aprobacion ni firma.**

La politica, privacidad y roles estan en [CODE_SIGNING.md](CODE_SIGNING.md).
Los pasos de publicacion y solicitud estan en [SIGNPATH_SETUP.md](SIGNPATH_SETUP.md).
Las respuestas para el formulario estan en [SIGNPATH_APPLICATION.md](SIGNPATH_APPLICATION.md);
el envio todavia requiere los datos, consentimientos y reCAPTCHA del responsable.
No hay certificados ni claves privadas incluidos en este repositorio.

La firma solo se solicita manualmente desde la rama predeterminada, una vez
configurado SignPath. No se firman automaticamente contribuciones externas.
Publicar el codigo en GitHub no hace que Windows confie en el ejecutable.

## Licencia

[MIT](LICENSE).
