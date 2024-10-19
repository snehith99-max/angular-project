import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTeleteamviewComponent } from './crm-trn-teleteamview.component';

describe('CrmTrnTeleteamviewComponent', () => {
  let component: CrmTrnTeleteamviewComponent;
  let fixture: ComponentFixture<CrmTrnTeleteamviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTeleteamviewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTeleteamviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
