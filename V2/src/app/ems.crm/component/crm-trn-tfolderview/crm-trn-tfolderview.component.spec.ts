import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CrmTrnTfolderviewComponent } from './crm-trn-tfolderview.component';

describe('CrmTrnTfolderviewComponent', () => {
  let component: CrmTrnTfolderviewComponent;
  let fixture: ComponentFixture<CrmTrnTfolderviewComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [CrmTrnTfolderviewComponent]
    });
    fixture = TestBed.createComponent(CrmTrnTfolderviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
