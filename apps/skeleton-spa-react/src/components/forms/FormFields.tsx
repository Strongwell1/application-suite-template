import type { InputHTMLAttributes, ReactNode, TextareaHTMLAttributes } from 'react'

type FieldShellProps = {
  children?: ReactNode
  helpText?: string
  label: string
  required?: boolean
}

function FieldShell({ children, helpText, label, required }: FieldShellProps) {
  return (
    <label className="form-field">
      <span>
        {label}
        {required && <b aria-label="required">*</b>}
      </span>
      {children}
      {helpText && <small>{helpText}</small>}
    </label>
  )
}

type TextFieldProps = InputHTMLAttributes<HTMLInputElement> & FieldShellProps

export function TextField({ children, helpText, label, required, ...inputProps }: TextFieldProps) {
  return (
    <FieldShell helpText={helpText} label={label} required={required}>
      <input {...inputProps} required={required} />
      {children}
    </FieldShell>
  )
}

type TextAreaFieldProps = TextareaHTMLAttributes<HTMLTextAreaElement> & FieldShellProps

export function TextAreaField({
  children,
  helpText,
  label,
  required,
  ...textareaProps
}: TextAreaFieldProps) {
  return (
    <FieldShell helpText={helpText} label={label} required={required}>
      <textarea {...textareaProps} required={required} />
      {children}
    </FieldShell>
  )
}
