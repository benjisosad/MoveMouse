# Code signing policy

## Estado actual

Este proyecto esta preparado para solicitar el programa de codigo abierto de
SignPath Foundation, pero **todavia no esta aprobado**. No se distribuyen aqui
certificados ni se afirma que los ejecutables actuales tengan una firma de confianza.
La aprobacion no esta garantizada, especialmente para proyectos nuevos sin
reputacion verificable.

## Equipo

- Autores y mantenedores: [@benjisosad](https://github.com/benjisosad).
- Revisores: [@benjisosad](https://github.com/benjisosad).
- Aprobadores de firma: [@benjisosad](https://github.com/benjisosad).

Todos los miembros deben tener autenticacion multifactor activada. Las
contribuciones externas deben revisarse antes de incorporarse. Cada solicitud
de firma de una version requiere aprobacion manual en SignPath.
La activacion de MFA debe confirmarse con el responsable antes de enviar la
solicitud; no se deduce de la existencia del repositorio publico.

## Origen de los binarios y restricciones

- Solo se firman binarios de este proyecto, compilados desde su codigo fuente.
- Los trabajos de compilacion y firma se ejecutan en runners hospedados por GitHub.
- La firma se solicita manualmente desde la rama predeterminada.
- SignPath debe verificar el origen del artefacto y limitar el producto a
  `MoveMouse`, el archivo a `MoveMouse.exe` y su version a la de la compilacion.
- Las claves privadas permanecen bajo la custodia del servicio de firma.
- Los tokens de API solo se almacenan como secretos de GitHub, nunca en el codigo.
- El entorno `signpath` debe restringirse a la rama predeterminada y configurarse
  con las revisiones disponibles para el repositorio.

## Privacidad

This program will not transfer any information to other networked systems
unless specifically requested by the user or the person installing or operating it.

La aplicacion no implementa telemetria, actualizaciones automaticas ni conexiones
de red. Durante la compilacion y firma, GitHub y SignPath procesan el codigo,
los artefactos y los metadatos de compilacion segun sus propias politicas:

- [Privacidad de GitHub](https://docs.github.com/en/site-policy/privacy-policies/github-general-privacy-statement)
- [Privacidad de SignPath](https://signpath.org/privacy-policy)

## Atribucion tras la aprobacion

Solo cuando el proyecto haya sido aceptado y use ese servicio, actualizar el
estado de esta pagina y publicar esta atribucion en la pagina principal y en
las publicaciones firmadas:

> Free code signing provided by [SignPath.io](https://signpath.io), certificate by [SignPath Foundation](https://signpath.org)
