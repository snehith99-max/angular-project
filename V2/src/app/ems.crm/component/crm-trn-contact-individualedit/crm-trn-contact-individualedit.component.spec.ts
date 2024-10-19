import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactIndividualeditComponent } from './crm-trn-contact-individualedit.component';

describe('CrmTrnContactIndividualeditComponent', () => {
  let component: CrmTrnContactIndividualeditComponent;
  let fixture: ComponentFixture<CrmTrnContactIndividualeditComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactIndividualeditComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactIndividualeditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
