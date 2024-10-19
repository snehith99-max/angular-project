import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmGmailinboxComponent } from './crm-smm-gmailinbox.component';

describe('CrmSmmGmailinboxComponent', () => {
  let component: CrmSmmGmailinboxComponent;
  let fixture: ComponentFixture<CrmSmmGmailinboxComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmGmailinboxComponent]
    });
    fixture = TestBed.createComponent(CrmSmmGmailinboxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
