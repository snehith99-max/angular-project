import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmSmmMailscomposeComponent } from './crm-smm-mailscompose.component';

describe('CrmSmmMailscomposeComponent', () => {
  let component: CrmSmmMailscomposeComponent;
  let fixture: ComponentFixture<CrmSmmMailscomposeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmSmmMailscomposeComponent]
    });
    fixture = TestBed.createComponent(CrmSmmMailscomposeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
