import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnContactIndividualViewComponent } from './crm-trn-contact-individual-view.component';

describe('CrmTrnContactIndividualViewComponent', () => {
  let component: CrmTrnContactIndividualViewComponent;
  let fixture: ComponentFixture<CrmTrnContactIndividualViewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnContactIndividualViewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnContactIndividualViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
