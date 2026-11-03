# Preparar la solicitud de SignPath

Repositorio: https://github.com/benjisosad/MoveMouse.
La preparacion del repositorio no equivale a una solicitud enviada o aprobada.
Aun no hay ejecutable firmado.

## 1. Revisar y publicar

1. Confirma que puedes publicar el codigo con la licencia MIT incluida. Ajusta
   el titular del copyright si corresponde.
2. Elige un nombre de repositorio que no se confunda con otros proyectos Move Mouse.
3. Completa los responsables en `CODE_SIGNING.md` y activa MFA en sus cuentas.
4. Crea un repositorio **publico** en tu cuenta de GitHub y publica estos archivos,
   incluidos `.github/workflows/build.yml` y `signpath/artifact-configuration.xml`.
   No subas certificados, claves ni tokens. El `.gitignore` excluye el ejecutable
   antiguo y los directorios de salida.
5. Comprueba que el workflow **Build and optional signing** termina correctamente.
6. Publica una primera version con el artefacto `MoveMouse-unsigned`, indicando
   expresamente que no esta firmado y puede ser bloqueado por Windows. Incluye
   la funcionalidad y las limitaciones documentadas en el README y en LEEME.txt.

SignPath exige un proyecto ya publicado, mantenido y con reputacion verificable.
Estos archivos no aseguran que un proyecto recien creado sea aceptado.

## 2. Solicitar el programa gratuito

Revisa los [requisitos actuales](https://signpath.org/terms) y usa el
[formulario oficial](https://signpath.org/apply).

Texto orientativo para completar con datos reales:

> Project: MoveMouse (independent project, not affiliated with sw3103/movemouse).
> Repository: https://github.com/benjisosad/MoveMouse
> License: MIT.
> Description: Small Windows Forms utility that sends continuous relative mouse
> movement using SendInput, with a global Space hotkey to stop it. It does not
> inject code, bypass anti-cheat, install services, or communicate over the network.
> Intended use disclosed by the repository owner: "For Fortnite EXP Farm".
> The application cannot guarantee game compatibility or compliance with game rules.
> Build: .NET Framework compiler on GitHub-hosted Windows runners.
> Signing: MoveMouse.exe inside a GitHub Actions ZIP artifact; source and build
> workflow are public. Each release will require manual signing approval.
> Maintainers/reviewers/approvers: https://github.com/benjisosad
> Releases and project history: PENDING_RELEASE_URL_AND_ACTUAL_HISTORY

No inventes antiguedad, usuarios, reputacion ni aprobaciones. SignPath puede pedir
informacion adicional o rechazar la solicitud.

## 3. Configurar la firma solo despues de ser aceptado

1. En SignPath, configura el proyecto y enlazalo al repositorio mediante la
   integracion de GitHub. Instala la GitHub App de SignPath si lo requieren.
2. Importa `signpath/artifact-configuration.xml` como configuracion del artefacto.
   La entrada es un ZIP creado por `actions/upload-artifact`, con `MoveMouse.exe`
   en su raiz. Se exige el parametro `version` y se restringen sus metadatos.
3. Configura una politica de firma con **aprobacion manual**, los aprobadores
   reales y las restricciones que indique SignPath. No desactives sus controles.
4. En GitHub, crea el entorno **signpath** y restringelo a la rama predeterminada.
   Configura revisores requeridos si tu plan lo permite.
5. Dentro de ese entorno, configura estas variables con los valores asignados:

   | Variable | Valor |
   | --- | --- |
   | `SIGNPATH_ORGANIZATION_ID` | ID de la organizacion en SignPath |
   | `SIGNPATH_PROJECT_SLUG` | Slug del proyecto |
   | `SIGNPATH_SIGNING_POLICY_SLUG` | Slug de la politica con aprobacion manual |
   | `SIGNPATH_ARTIFACT_CONFIGURATION_SLUG` | Slug de la configuracion XML importada |

6. Anade el secreto de entorno **SIGNPATH_API_TOKEN**, con permisos minimos para
   solicitar firmas en ese proyecto. No lo pegues en issues, commits o chats.
7. Actualiza la atribucion y el estado de `README.md` y `CODE_SIGNING.md` solo
   cuando la aprobacion sea real.

## 4. Solicitar y publicar una version firmada

1. Actualiza las dos versiones de `AssemblyInfo.cs` y revisa los cambios.
2. En Actions, ejecuta **Build and optional signing** mediante **Run workflow**
   desde la rama predeterminada y marca **Request SignPath signing**.
3. Aprueba la ejecucion del entorno si corresponde y la solicitud en SignPath.
   El workflow espera hasta 30 minutos a la aprobacion de firma; si vence el
   plazo, consulta la solicitud en SignPath antes de iniciar otra.
4. El job de firma comprueba Authenticode y regenera el hash del ejecutable firmado.
   Publica el artefacto **MoveMouse-signed**, no el artefacto unsigned.
5. Conserva el enlace a la ejecucion y al commit utilizado en la publicacion.

La compilacion local sigue sin estar firmada. Incluso con firma, no se garantiza
que cualquier configuracion o politica de seguridad permita ejecutar la app.
No se modifica el Control inteligente de aplicaciones.
